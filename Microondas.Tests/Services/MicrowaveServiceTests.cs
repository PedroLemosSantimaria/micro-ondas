using System.Linq;
using Microondas.Core.Exceptions;
using Microondas.Core.Interfaces;
using Microondas.Core.Services;
using Microondas.Tests.Helpers;
using Moq;
using Xunit;

namespace Microondas.Tests.Services
{
    public class MicrowaveServiceTests
    {
        private readonly Mock<IHeatingProgramRepository> heatingProgramRepositoryMock;
        private readonly MicrowaveService microwaveService;

        public MicrowaveServiceTests()
        {
            heatingProgramRepositoryMock = new Mock<IHeatingProgramRepository>();

            heatingProgramRepositoryMock
                .Setup(x => x.GetAllPrograms())
                .Returns(FakeData.GetPrograms());

            heatingProgramRepositoryMock
                .Setup(x => x.GetProgramByName(It.IsAny<string>()))
                .Returns((string name) =>
                    FakeData.GetPrograms().FirstOrDefault(x => x.Name == name));

            microwaveService = new MicrowaveService(heatingProgramRepositoryMock.Object);
        }

        [Fact]
        public void StartQuick_ShouldCreateSessionWith30SecondsAndPower10()
        {
            var result = microwaveService.StartQuick();

            Assert.NotNull(result);
            Assert.Equal(30, result.TotalTimeInSeconds);
            Assert.Equal(30, result.RemainingTimeInSeconds);
            Assert.Equal(10, result.Power);
            Assert.True(result.IsRunning);
            Assert.False(result.IsPaused);
        }

        [Fact]
        public void StartManual_WithValidValues_ShouldCreateSession()
        {
            var result = microwaveService.StartManual(20, 3);

            Assert.NotNull(result);
            Assert.Equal(20, result.TotalTimeInSeconds);
            Assert.Equal(20, result.RemainingTimeInSeconds);
            Assert.Equal(3, result.Power);
            Assert.Equal(".", result.HeatingChar);
            Assert.True(result.IsRunning);
        }

        [Fact]
        public void StartManual_WithInvalidTime_ShouldThrowBusinessException()
        {
            Assert.Throws<BusinessException>(() =>
            {
                microwaveService.StartManual(0, 5);
            });
        }

        [Fact]
        public void StartManual_WithInvalidPower_ShouldThrowBusinessException()
        {
            Assert.Throws<BusinessException>(() =>
            {
                microwaveService.StartManual(20, 11);
            });
        }

        [Fact]
        public void StartManual_WhenAlreadyRunningManual_ShouldAdd30Seconds()
        {
            microwaveService.StartManual(20, 5);

            var result = microwaveService.StartManual(20, 5);

            Assert.Equal(50, result.TotalTimeInSeconds);
            Assert.Equal(50, result.RemainingTimeInSeconds);
        }

        [Fact]
        public void StartQuick_WhenAlreadyRunningManual_ShouldAdd30Seconds()
        {
            microwaveService.StartManual(15, 4);

            var result = microwaveService.StartQuick();

            Assert.Equal(45, result.TotalTimeInSeconds);
            Assert.Equal(45, result.RemainingTimeInSeconds);
        }

        [Fact]
        public void StartProgram_WhenAlreadyRunningProgram_AndTryingToStartAgain_ShouldThrowBusinessException()
        {
            microwaveService.StartProgram("Pipoca");

            Assert.Throws<BusinessException>(() =>
            {
                microwaveService.StartQuick();
            });
        }

        [Fact]
        public void PauseOrCancel_WhenRunning_ShouldPauseSession()
        {
            microwaveService.StartManual(20, 5);

            var result = microwaveService.PauseOrCancel();

            Assert.True(result.IsPaused);
            Assert.False(result.IsRunning);
        }

        [Fact]
        public void PauseOrCancel_WhenPaused_ShouldCancelSession()
        {
            microwaveService.StartManual(20, 5);
            microwaveService.PauseOrCancel();

            var result = microwaveService.PauseOrCancel();

            Assert.True(result.IsCancelled);
            Assert.False(result.IsRunning);
            Assert.False(result.IsPaused);
        }

        [Fact]
        public void Resume_WhenPaused_ShouldContinueSession()
        {
            microwaveService.StartManual(20, 5);
            microwaveService.PauseOrCancel();

            var result = microwaveService.Resume();

            Assert.True(result.IsRunning);
            Assert.False(result.IsPaused);
        }

        [Fact]
        public void Tick_ShouldDecreaseRemainingTime()
        {
            microwaveService.StartManual(10, 2);

            var result = microwaveService.Tick();

            Assert.Equal(9, result.RemainingTimeInSeconds);
            Assert.Contains("..", result.ProcessText);
        }

        [Fact]
        public void Tick_WhenTimeEnds_ShouldFinishHeating()
        {
            microwaveService.StartManual(1, 2);

            var result = microwaveService.Tick();

            Assert.True(result.IsFinished);
            Assert.False(result.IsRunning);
            Assert.Contains("Aquecimento concluído", result.ProcessText);
        }

        [Fact]
        public void StartProgram_ShouldCreateSessionUsingProgramData()
        {
            var result = microwaveService.StartProgram("Pipoca");

            Assert.NotNull(result);
            Assert.Equal(180, result.TotalTimeInSeconds);
            Assert.Equal(7, result.Power);
            Assert.Equal("*", result.HeatingChar);
            Assert.Equal("Pipoca", result.ProgramName);
        }

        [Fact]
        public void StartManual_WithoutTimeAndPower_ShouldUseQuickStart()
        {
            var result = microwaveService.StartManual(null, null);

            Assert.Equal(30, result.TotalTimeInSeconds);
            Assert.Equal(10, result.Power);
        }
    }
}