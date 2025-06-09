using NSubstitute;
using NUnit.Framework;
using RaidCoachingGroup1MatchScores.Enums;
using RaidCoachingGroup1MatchScores.Models;
using RaidCoachingGroup1MatchScores.Repositories;
using RaidCoachingGroup1MatchScores.Services;

namespace RaidCoachingGroup1MatchScores.Tests;

[TestFixture]
public class MatchServiceTests
{
    private IMatchRepository _matchRepository;
    private IMatchService _matchService;

    [SetUp]
    public void Setup()
    {
        _matchRepository = Substitute.For<IMatchRepository>();
        _matchService = new MatchService(_matchRepository);
    }

    [Test]
    public void UpdateMatchScore_WhenMatchDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var model = new UpdateMatchScoresModel { MatchId = 1 };
        _matchRepository.GetMatchScore(1).Returns((MatchScoreModel)null);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _matchService.UpdateMatchScore(model));
        Assert.That(ex.Message, Is.EqualTo("Match with ID 1 does not exist"));
    }

    [Test]
    public void UpdateMatchScore_WithHomeGoal_UpdatesScoreCorrectly()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.HomeGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "H" });

        // Act
        var result = _matchService.UpdateMatchScore(model);

        // Assert
        _matchRepository.Received(1).UpdateMatchScore(Arg.Is<UpdateMatchScoresRepoModel>(m => 
            m.MatchId == 1 && m.Scores == "HH"));
        Assert.That(result, Is.EqualTo("2:0 (First Half)"));
    }

    [Test]
    public void UpdateMatchScore_WithAwayGoal_UpdatesScoreCorrectly()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.AwayGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "A" });

        // Act
        var result = _matchService.UpdateMatchScore(model);

        // Assert
        _matchRepository.Received(1).UpdateMatchScore(Arg.Is<UpdateMatchScoresRepoModel>(m => 
            m.MatchId == 1 && m.Scores == "AA"));
        Assert.That(result, Is.EqualTo("0:2 (First Half)"));
    }

    [Test]
    public void UpdateMatchScore_WithNextPeriod_UpdatesScoreCorrectly()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.NextPeriod
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HHA" });

        // Act
        var result = _matchService.UpdateMatchScore(model);

        // Assert
        _matchRepository.Received(1).UpdateMatchScore(Arg.Is<UpdateMatchScoresRepoModel>(m => 
            m.MatchId == 1 && m.Scores == "HHA;"));
        Assert.That(result, Is.EqualTo("2:1 (Second Half)"));
    }

    [Test]
    public void UpdateMatchScore_WithCancelHomeGoal_UpdatesScoreCorrectly()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.CancelHomeGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HHA" });

        // Act
        var result = _matchService.UpdateMatchScore(model);

        // Assert
        _matchRepository.Received(1).UpdateMatchScore(Arg.Is<UpdateMatchScoresRepoModel>(m => 
            m.MatchId == 1 && m.Scores == "HA"));
        Assert.That(result, Is.EqualTo("1:1 (First Half)"));
    }

    [Test]
    public void UpdateMatchScore_WithCancelAwayGoal_UpdatesScoreCorrectly()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.CancelAwayGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HHA" });

        // Act
        var result = _matchService.UpdateMatchScore(model);

        // Assert
        _matchRepository.Received(1).UpdateMatchScore(Arg.Is<UpdateMatchScoresRepoModel>(m => 
            m.MatchId == 1 && m.Scores == "HH"));
        Assert.That(result, Is.EqualTo("2:0 (First Half)"));
    }

    [Test]
    public void UpdateMatchScore_CancelHomeGoal_WhenLastGoalWasAway_ThrowsException()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.CancelHomeGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HHA" });

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _matchService.UpdateMatchScore(model));
        Assert.That(ex.Message, Is.EqualTo("Cannot cancel home goal when last goal was away"));
    }

    [Test]
    public void UpdateMatchScore_CancelAwayGoal_WhenLastGoalWasHome_ThrowsException()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.CancelAwayGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HAH" });

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _matchService.UpdateMatchScore(model));
        Assert.That(ex.Message, Is.EqualTo("Cannot cancel away goal when last goal was home"));
    }

    [Test]
    public void UpdateMatchScore_NextPeriod_WhenAlreadyInSecondHalf_ThrowsException()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.NextPeriod
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "HA;H" });

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _matchService.UpdateMatchScore(model));
        Assert.That(ex.Message, Is.EqualTo("Cannot advance to next period when already in second half"));
    }

    [Test]
    public void UpdateMatchScore_CancelGoal_WhenNoGoalsScored_ThrowsException()
    {
        // Arrange
        var model = new UpdateMatchScoresModel 
        { 
            MatchId = 1,
            MatchEvent = MatchEventEnum.CancelHomeGoal
        };
        
        _matchRepository.GetMatchScore(1).Returns(new MatchScoreModel { Score = "" });

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _matchService.UpdateMatchScore(model));
        Assert.That(ex.Message, Is.EqualTo("Cannot cancel goal when no goals have been scored"));
    }
} 