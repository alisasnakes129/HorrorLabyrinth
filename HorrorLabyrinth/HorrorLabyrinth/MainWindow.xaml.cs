using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Media;

namespace HorrorLabyrinth
{
    public partial class MainWindow : Window
    {
        private string playerName = "Герой";
        private int currentStep = 0;

        private SoundPlayer bgmPlayer = new SoundPlayer();
        private SoundPlayer sfxPlayer = new SoundPlayer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtPlayerName.Text))
            {
                playerName = TxtPlayerName.Text.Trim();
            }

            NamePanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;

            PlayBackgroundMusic();
            LoadRoadScene();
        }

        private void PlayBackgroundMusic()
        {
            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "ambient.wav");
                bgmPlayer.SoundLocation = path;
                bgmPlayer.PlayLooping();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка звука эмбиента: " + ex.Message);
            }
        }

        private void LoadRoadScene()
        {
            currentStep = 1;
            FadeImage("Images/road.jpg");
            ImgPlayer.Visibility = Visibility.Collapsed;
            ImgCharacter.Visibility = Visibility.Collapsed;
            ChoicePanelStep1.Visibility = Visibility.Collapsed;
            ChoicePanelStep2.Visibility = Visibility.Collapsed;
            ChoicePanelGate.Visibility = Visibility.Collapsed;
            BtnNext.Visibility = Visibility.Visible;
            BtnNext.Content = "Далее ➔";

            TxtDialogue.Text = $"{playerName} ведет машину сквозь непроглядную ночную мглу... Фары выхватывают из темноты лишь клубы густого тумана. Вокруг ни единой живой души.";
        }

        private void FadeImage(string imagePath)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                ImgRoom.Source = bitmap;

                DoubleAnimation fadeIn = new DoubleAnimation(0.2, 1.0, TimeSpan.FromSeconds(0.6));
                ImgRoom.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            catch (Exception) { }
        }

        // Обычный скример
        private void TriggerJumpscare(string text)
        {
            FadeImage("Images/jumpscare.jpg");
            ImgPlayer.Visibility = Visibility.Collapsed;
            ImgCharacter.Visibility = Visibility.Collapsed;
            ChoicePanelStep1.Visibility = Visibility.Collapsed;
            ChoicePanelStep2.Visibility = Visibility.Collapsed;
            ChoicePanelGate.Visibility = Visibility.Collapsed;

            try
            {
                bgmPlayer.Stop();
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "screamer.wav");
                sfxPlayer.SoundLocation = path;
                sfxPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка звука скримера: " + ex.Message);
            }

            TxtDialogue.Text = text;
            BtnNext.Content = "Начать заново";
            BtnNext.Visibility = Visibility.Visible;
            currentStep = 99;
        }

        // Финальный скример для труса (jumpscare_secret.jpg + jumpscare_final.wav)
        private void TriggerFinalJumpscare(string text)
        {
            FadeImage("Images/jumpscare_secret.jpg");
            ImgPlayer.Visibility = Visibility.Collapsed;
            ImgCharacter.Visibility = Visibility.Collapsed;
            ChoicePanelStep1.Visibility = Visibility.Collapsed;
            ChoicePanelStep2.Visibility = Visibility.Collapsed;
            ChoicePanelGate.Visibility = Visibility.Collapsed;

            try
            {
                bgmPlayer.Stop();
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "jumpscare_final.wav");
                sfxPlayer.SoundLocation = path;
                sfxPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка финального звука: " + ex.Message);
            }

            TxtDialogue.Text = text;
            BtnNext.Content = "Начать заново";
            BtnNext.Visibility = Visibility.Visible;
            currentStep = 99;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep == 99)
            {
                bgmPlayer.Stop();
                NamePanel.Visibility = Visibility.Visible;
                GamePanel.Visibility = Visibility.Collapsed;
                ImgRoom.Source = null;
                ImgPlayer.Visibility = Visibility.Collapsed;
                ImgCharacter.Visibility = Visibility.Collapsed;
                TxtPlayerName.Clear();
                currentStep = 0;
                return;
            }

            if (currentStep == 10)
            {
                TxtDialogue.Text = $"{playerName}: — Эй... ты слышишь меня? Отзовись!\n\nВ ответ лишь гробовая тишина. Пассажирское сиденье пусто. Куда она делась? Что делать дальше?";
                BtnNext.Visibility = Visibility.Collapsed;
                ChoicePanelStep2.Visibility = Visibility.Visible;
                return;
            }

            // Диалог у ворот со стражем
            if (currentStep == 50)
            {
                TxtDialogue.Text = $"Страж смотрит на тебя с издевкой: — Ты что тут забыл, бессмертный что ли?";
                currentStep = 51;
                return;
            }
            else if (currentStep == 51)
            {
                try
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "laugh.wav");
                    sfxPlayer.SoundLocation = path;
                    sfxPlayer.Play();
                }
                catch (Exception) { }

                TxtDialogue.Text = $"{playerName}: — Я попал в аварию и потерял свою девушку...\n\nСтраж: — АХАХХААХ, ну ты выдумщик! Скройся с глаз моих!";
                currentStep = 52;
                BtnNext.Content = "Ответить стражу ➔";
                return;
            }
            else if (currentStep == 52)
            {
                TxtDialogue.Text = $"Что делать дальше?";
                BtnNext.Visibility = Visibility.Collapsed;
                ChoicePanelGate.Visibility = Visibility.Visible;
                return;
            }

            currentStep++;

            if (currentStep == 2)
            {
                FadeImage("Images/crash.jpg");
                ImgPlayer.Visibility = Visibility.Collapsed;
                ImgCharacter.Visibility = Visibility.Collapsed;

                TxtDialogue.Text = $"{playerName} внезапно из тумана прямо на дорогу выбегает силуэт! Резкий выворот руля... Удар! Машина врезается в вековой дуб. Темнота...";

                BtnNext.Visibility = Visibility.Collapsed;
                ChoicePanelStep1.Visibility = Visibility.Visible;
            }
        }

        private void BtnChoiceOpenEyes_Click(object sender, RoutedEventArgs e)
        {
            FadeImage("Images/crash.jpg");
            ImgPlayer.Source = new BitmapImage(new Uri("Images/player.png", UriKind.Relative));
            ImgPlayer.Visibility = Visibility.Visible;
            ChoicePanelStep1.Visibility = Visibility.Collapsed;

            TxtDialogue.Text = $"{playerName} с трудом распахивает глаза. Голова раскалывается, салон затянут дымом.";
            BtnNext.Content = "Окликнуть девушку ➔";
            BtnNext.Visibility = Visibility.Visible;
            currentStep = 10;
        }

        private void BtnChoiceStayWeak_Click(object sender, RoutedEventArgs e)
        {
            TriggerJumpscare($"{playerName} решает не открывать глаза и остаться неподвижным... Внезапно чьи-то ледяные пальцы вцепляются в шею! А-А-А! ИГРА ОКОНЧЕНА.");
        }

        private void BtnChoiceSearchGirl_Click(object sender, RoutedEventArgs e)
        {
            FadeImage("Images/entrance_closed.png");

            ImgPlayer.Source = new BitmapImage(new Uri("Images/player.png", UriKind.Relative));
            ImgPlayer.Visibility = Visibility.Visible;

            ImgCharacter.Source = new BitmapImage(new Uri("Images/gatekeeper.png", UriKind.Relative));
            ImgCharacter.Visibility = Visibility.Visible;

            ChoicePanelStep2.Visibility = Visibility.Collapsed;

            TxtDialogue.Text = $"{playerName} выбирается из разбитой машины и спешит сквозь туман. Перед ним вырастают кованые ворота замка, у которых стоит жуткий Страж.";

            BtnNext.Content = "Продолжить разговор ➔";
            BtnNext.Visibility = Visibility.Visible;
            currentStep = 50;
        }

        private void BtnChoiceInspectCar_Click(object sender, RoutedEventArgs e)
        {
            TriggerJumpscare($"{playerName} решает проигнорировать исчезновение и наклоняется к капоту... Из темноты салона вырывается нечто ужасное! А-А-А! ИГРА ОКОНЧЕНА.");
        }

        private void BtnGateInsist_Click(object sender, RoutedEventArgs e)
        {
            ChoicePanelGate.Visibility = Visibility.Collapsed;
            // Открытие ворот с новым именем файла entrance_open.png
            FadeImage("Images/entrance_open.png");

            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "gates.wav");
                sfxPlayer.SoundLocation = path;
                sfxPlayer.Play();
            }
            catch (Exception) { }

            TxtDialogue.Text = $"Страж меняется в лице: — Хорошо, ты убедил меня... Иди с богом.\n\nТяжелые створки ворот со скрипом открываются перед {playerName}. Путь свободен!";
            BtnNext.Content = "Начать заново";
            BtnNext.Visibility = Visibility.Visible;
            currentStep = 99;
        }

        private void BtnGateCoward_Click(object sender, RoutedEventArgs e)
        {
            ChoicePanelGate.Visibility = Visibility.Collapsed;
            // Скример для труса с картинкой jumpscare_secret.jpg и звуком jumpscare_final.wav
            TriggerFinalJumpscare($"Страж смеется в голос: — Я и не сомневался, лошара!\n\nВнезапно он набрасывается из темноты! А-А-А! ИГРА ОКОНЧЕНА.");
        }

        private void BtnChoiceGoal_Click(object sender, RoutedEventArgs e)
        {
            TxtDialogue.Text = "ЦЕЛЬ ИГРЫ: Найти пропавшую спутницу и выбраться из проклятого замка, избегая смертельных ловушек.";
        }
    }
}