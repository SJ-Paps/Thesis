using NSubstitute;
using NUnit.Framework;
using SJ.Management.Audio;
using SJ.Management;
using SJ.Menu;
using System;
using UniRx;
using UnityEngine.Events;

namespace SJ.Tests
{
    public class SoundMenuViewControllerShould
    {
        private const float GeneralVolume = 0.1f;
        private const float MusicVolume = 0.2f;
        private const float EffectsVolume = 0.3f;

        private ISoundSettingsScreenView view;
        private ISoundService soundService;
        private IGameSettingsRepository gameSettingsRepository;
        private IOptionsScreenView optionsScreenView;

        private SoundSettingsScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<ISoundSettingsScreenView>();
            soundService = Substitute.For<ISoundService>();
            gameSettingsRepository = Substitute.For<IGameSettingsRepository>();
            optionsScreenView = Substitute.For<IOptionsScreenView>();
        }

        [Test]
        public void Subscribe_To_Slider_Events()
        {
            GivenAController();

            view.Received(1).OnGeneralSoundValueChanged += Arg.Any<Action<float>>();
            view.Received(1).OnMusicSoundValueChanged += Arg.Any<Action<float>>();
            view.Received(1).OnEffectsSoundValueChanged += Arg.Any<Action<float>>();

            view.Received(1).OnGeneralSoundValueConfirmed += Arg.Any<Action<float>>();
            view.Received(1).OnMusicSoundValueConfirmed += Arg.Any<Action<float>>();
            view.Received(1).OnEffectsSoundValueConfirmed += Arg.Any<Action<float>>();
        }

        [Test]
        public void Set_Initial_Volume_Values()
        {
            soundService.GetVolume().Returns(GeneralVolume);
            soundService.GetVolumeOfChannel(SoundChannels.Music).Returns(MusicVolume);
            soundService.GetVolumeOfChannel(SoundChannels.Effects).Returns(EffectsVolume);

            GivenAController();

            view.Received(1).SetGeneralSoundValue(GeneralVolume);
            view.Received(1).SetMusicSoundValue(MusicVolume);
            view.Received(1).SetEffectsSoundValue(EffectsVolume);
        }

        [Test]
        public void Update_Corresponding_Volume_Value_When_A_Slider_Value_Changes()
        {
            GivenAController();

            view.OnGeneralSoundValueChanged += Raise.Event<Action<float>>(GeneralVolume);
            view.OnMusicSoundValueChanged += Raise.Event<Action<float>>(MusicVolume);
            view.OnEffectsSoundValueChanged += Raise.Event<Action<float>>(EffectsVolume);

            soundService.Received(1).SetVolume(GeneralVolume);
            soundService.Received(1).SetVolumeOfChannel(SoundChannels.Music, MusicVolume);
            soundService.Received(1).SetVolumeOfChannel(SoundChannels.Effects, EffectsVolume);
        }

        [Test]
        public void Save_On_Game_Settings_The_Corresponding_Volume_Value_When_It_Has_Been_Confirmed()
        {
            GivenAController();

            var gameSettings = new GameSettings();

            gameSettingsRepository.GetSettings().Returns(Observable.Return(gameSettings));
            gameSettingsRepository.SaveSettings().Returns(Observable.ReturnUnit());

            view.OnGeneralSoundValueConfirmed += Raise.Event<Action<float>>(GeneralVolume);
            view.OnMusicSoundValueConfirmed += Raise.Event<Action<float>>(MusicVolume);
            view.OnEffectsSoundValueConfirmed += Raise.Event<Action<float>>(EffectsVolume);

            gameSettingsRepository.Received(3).GetSettings();
            gameSettingsRepository.Received(3).SaveSettings();

            Assert.That(gameSettings.generalVolume == GeneralVolume, "General volume has been updated");
            Assert.That(gameSettings.musicVolume == MusicVolume, "Music volume has been updated");
            Assert.That(gameSettings.effectsVolume == EffectsVolume, "Effects volume has been updated");
        }

        [Test]
        public void Return_To_Main_Screen_When_Back_Button_Is_Clicked()
        {
            GivenAController();

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            optionsScreenView.Received(1).FocusMainScreen();
        }

        private void GivenAController()
        {
            controller = new SoundSettingsScreenViewController(view, gameSettingsRepository, soundService, optionsScreenView);
        }
    }
}


