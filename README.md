# Micro-ondas Digital

Aplicação web de simulação de um micro-ondas digital desenvolvida como teste técnico, com regras de aquecimento, programas pré-definidos/customizados, autenticação e API para a camada de negócio.

## Visão geral

Este projeto implementa um micro-ondas digital com interface web e back-end em API, seguindo os requisitos da avaliação proposta. A solução foi separada em duas partes:

* **Back-end**: responsável pelas regras de negócio, autenticação, controle da sessão de aquecimento, programas de aquecimento e persistência dos programas customizados.
* **Front-end**: responsável pela interface do usuário, painel do micro-ondas, login, acionamento das funções e cadastro de programas customizados.

A implementação foi organizada em camadas para separar interface e regra de negócio, priorizando orientação a objetos, legibilidade e facilidade de manutenção.

---

# Nível atendido

A solução entregue atende ao **Nível 4** da avaliação, contemplando:

* **Nível 1**

  * Aquecimento manual com tempo e potência
  * Início rápido
  * Acréscimo de tempo durante aquecimento
  * Pausa, retomada e cancelamento
  * String de aquecimento durante o processamento
  * Validações de tempo e potência

* **Nível 2**

  * Programas de aquecimento pré-definidos
  * Bloqueio de alteração de tempo/potência nos programas pré-definidos
  * Restrições de acréscimo de tempo para programas pré-definidos

* **Nível 3**

  * Cadastro de programas customizados
  * Validação de caractere de aquecimento
  * Persistência de programas customizados em arquivo JSON
  * Exibição conjunta de programas pré-definidos e customizados

* **Nível 4**

  * Regras expostas via Web API
  * Autenticação com Bearer Token
  * Tela de login e bloqueio de acesso às funções sem autenticação
  * Tratamento padronizado de exceptions
  * Exception específica para regra de negócio
  * Log de erros em arquivo

---

# Tecnologias utilizadas

## Back-end

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**
* **JWT Bearer Authentication**
* **JSON para persistência de programas customizados**

## Front-end

* **React**
* **Vite**
* **Axios**
* **CSS puro**

---

# Estrutura do projeto

```text
Microondas/
├─ Microondas.Api/                # API ASP.NET Core
├─ Microondas.Core/               # regras de negócio, entidades, contratos e services
├─ Microondas.Infrastructure/     # persistência, auth, logs e repositórios
├─ microondas-web/                # front-end React
└─ README.md
```

## Resumo das camadas

### `Microondas.Core`

Contém:

* entidades do domínio
* DTOs/requests/responses
* interfaces
* exceptions de negócio
* services principais do micro-ondas e dos programas

### `Microondas.Infrastructure`

Contém:

* repositório dos programas
* persistência em JSON
* autenticação/JWT
* logger de erro em arquivo

### `Microondas.Api`

Contém:

* controllers
* configuração da API
* autenticação
* middlewares
* tratamento global de erros

### `microondas-web`

Contém:

* tela de login
* painel do micro-ondas
* listagem de programas
* cadastro de programa customizado
* integração com a API

---

# Funcionalidades

## Aquecimento manual

Permite informar:

* tempo
* potência

Regras principais:

* tempo mínimo de **1 segundo**
* tempo máximo de **2 minutos** para aquecimento manual
* potência entre **1 e 10**
* potência padrão **10** quando não informada
* início rápido com **30 segundos** e potência **10**

## Controle do aquecimento

O sistema permite:

* iniciar aquecimento
* pausar
* retomar
* cancelar
* adicionar 30 segundos ao tempo restante quando aplicável
* acompanhar a string de aquecimento sendo construída durante a execução

## Programas pré-definidos

Foram implementados os programas:

* Pipoca
* Leite
* Carnes de boi
* Frango
* Feijão

Cada um possui:

* nome
* alimento
* tempo
* potência
* caractere de aquecimento
* instruções

## Programas customizados

É possível cadastrar programas personalizados com:

* nome
* alimento
* tempo
* potência
* caractere de aquecimento
* instruções opcionais

Regras:

* o caractere não pode ser `.`
* o caractere não pode se repetir com outros programas
* programas customizados são persistidos em arquivo JSON

## Autenticação

A API utiliza autenticação com **Bearer Token**.

O front possui uma tela de login e só libera o uso do micro-ondas após autenticação com sucesso.

## Tratamento de erros

Foi implementado:

* tratamento global de exceptions
* exception específica para regra de negócio
* retorno padronizado de erro
* log de erros em arquivo texto

---

# Endpoints principais da API

## Autenticação

* `POST /api/auth/login`

## Programas

* `GET /api/programs`
* `POST /api/programs/custom`

## Micro-ondas

* `POST /api/microwave/start`
* `POST /api/microwave/quick-start`
* `POST /api/microwave/pause-cancel`
* `POST /api/microwave/resume`
* `POST /api/microwave/tick`
* `POST /api/microwave/start-program`
* `GET /api/microwave/session`

---

# Como executar o projeto

## Pré-requisitos

Instale:

* **.NET SDK 8 ou superior**
* **Node.js 18+**
* **npm**
* **Visual Studio 2022** ou superior (recomendado para o back-end)

---

# 1. Clonar o repositório

```bash
git clone https://github.com/PedroLemosSantimaria/micro-ondas.git
```

---

# 2. Rodar o back-end

## Pelo Visual Studio

1. Abra a solução no Visual Studio
2. Defina o projeto **Microondas.Api** como projeto de inicialização
3. Rode a aplicação

A API ficará disponível em uma URL parecida com:

```bash
http://localhost:5144
```

ou

```bash
https://localhost:7250
```

---

# 3. Rodar o front-end

Abra um terminal na pasta do front:

```bash
cd microondas-web
npm install
npm run dev
```

O front normalmente ficará disponível em algo como:

```bash
http://localhost:5173
```

---

# 4. Configurar a URL da API no front

No arquivo:

```bash
microondas-web/src/api/axiosClient.js
```

ajuste a `baseURL` conforme a porta usada pela API.

Exemplo:

```javascript
import axios from "axios";
import { getToken } from "../utils/storage";

const axiosClient = axios.create({
  baseURL: "http://localhost:5144",
});

axiosClient.interceptors.request.use((config) => {
  const token = getToken();

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default axiosClient;
```

---

# 5. Login para testes

O projeto utiliza autenticação. Para acessar a aplicação, utilize as credenciais configuradas no back-end.

Exemplo de usuário de teste:

```text
Usuário: admin
Senha: 123456
```

---

# Como usar

## Fluxo básico

1. Fazer login
2. Informar tempo e potência manualmente **ou** escolher um programa pré-definido
3. Iniciar o aquecimento
4. Acompanhar o processamento no painel
5. Usar pausa/cancelamento quando necessário
6. Cadastrar programas customizados no formulário lateral

## Comportamentos importantes

* Se iniciar sem informar tempo e potência, o sistema faz **início rápido**
* Se já existir aquecimento manual em andamento e o usuário iniciar novamente, o tempo é acrescido conforme a regra implementada
* Programas pré-definidos usam valores fixos
* Programas customizados são persistidos em arquivo JSON

---

# Regras e decisões de implementação

## Arquitetura

A solução foi separada em camadas para atender ao requisito de separar interface e negócio:

* front-end desacoplado da regra
* API responsável pelas operações
* services de domínio concentrando a lógica do micro-ondas

## Persistência dos programas customizados

Os programas customizados são salvos em arquivo JSON para simplificar a execução do projeto sem depender de banco de dados.

## Tick do aquecimento

O processamento do aquecimento é controlado por chamadas à API para atualizar a sessão do micro-ondas, permitindo refletir o tempo restante e a string de aquecimento na interface.

## Tratamento de erro

As regras de negócio lançam exceptions específicas e a API padroniza a resposta para o front.

---

# Melhorias possíveis

Alguns pontos que poderiam evoluir em uma próxima versão:

* testes unitários mais abrangentes no front-end
* persistência em banco SQL Server
* refresh automático da sessão com estratégia mais robusta
* tela de configuração das credenciais conforme o enunciado do nível 4
* melhoria visual do painel e responsividade
* cobertura de testes automatizados para fluxos da API

---

# Observações para avaliação

* O foco da implementação foi atender às regras funcionais da avaliação com separação entre interface e negócio.
* A interface foi mantida simples, priorizando o comportamento do micro-ondas e a integração com a API.
* O projeto foi desenvolvido com orientação a objetos e organização em camadas.

---

# Autor

Desenvolvido por **Pedro Lemos**.

---

# Challenge

This is a challenge by [Coodesh](https://coodesh.com/)
