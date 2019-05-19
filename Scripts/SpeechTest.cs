using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Voice
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "A", "print my name" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            
            recEngine.SetInputToDefaultAudioDevice();
            
            
            recEngine.SpeechRecognized += haveRecognized;
            recEngine.SpeechDetected += Test;
            recEngine.AudioSignalProblemOccurred += problems;
            MessageBox.Show(synthesizer.Volume.ToString());
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.SelectVoiceByHints(VoiceGender.Female);
            synthesizer.SpeakAsync("I'm loaded");
        }

        private void problems(object sender, AudioSignalProblemOccurredEventArgs e)
        {
            MessageBox.Show(e.AudioSignalProblem.ToString());
            synthesizer.SpeakAsync(e.AudioSignalProblem.ToString());
        }


        private void Test(object sender, SpeechDetectedEventArgs e)
        {
            MessageBox.Show("Ha");
            synthesizer.SpeakAsync("Ha");
            MessageBox.Show("Ha after");
        }

        private void haveRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show("have got something");
            switch (e.Result.Text)
            {
                case "A":
                    MessageBox.Show("hello Master");
                    synthesizer.SpeakAsync("Hello Master");
                    break;
                case "print my name":
                    richTextBox1.Text += "\n Jonny";
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            recEngine.Dispose();
        }
    }
}
