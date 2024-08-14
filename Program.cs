using ScreenRecorderLib; // Importa a biblioteca para gravação de tela / Imports the screen recording library
using System; // Importa funcionalidades básicas do sistema / Imports basic system functionalities
using System.IO; // Importa funcionalidades para manipulação de arquivos e diretórios / Imports functionalities for file and directory manipulation

class Program
{
	static void Main(string[] args)
	{
		// Gerar o nome do arquivo com data e hora atual / Generate the filename with current date and time
		string dateTimeFormat = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		string tempPath = Path.GetTempPath(); // Obter o caminho da pasta temporária do sistema / Get the system's temporary folder path
		string videoPath = Path.Combine(tempPath, $"screen_capture_{dateTimeFormat}.mp4");

		// Configurações do gravador / Recorder settings
		RecorderOptions options = new RecorderOptions
		{
			AudioOptions = new AudioOptions
			{
				IsAudioEnabled = true, // Habilita a captura de áudio / Enables audio capturing
				Bitrate = AudioBitrate.bitrate_128kbps, // Define a taxa de bits do áudio / Sets the audio bitrate
				Channels = AudioChannels.Stereo, // Define os canais de áudio como estéreo / Sets the audio channels to stereo
				IsOutputDeviceEnabled = true, // Habilita a captura do dispositivo de saída / Enables output device capturing
				IsInputDeviceEnabled = true // Habilita a captura do dispositivo de entrada (ex: microfone) / Enables input device capturing (e.g., microphone)
			},
			VideoEncoderOptions = new VideoEncoderOptions
			{
				Bitrate = 8000 * 1000, // Define a taxa de bits do vídeo (8 Mbps) / Sets the video bitrate (8 Mbps)
				Framerate = 30, // Define a taxa de quadros por segundo / Sets the frames per second
				IsFixedFramerate = true // Estabelece que a taxa de quadros é fixa / Establishes that the framerate is fixed
			},
			OutputOptions = new OutputOptions
			{
				IsVideoCaptureEnabled = true, // Habilita a captura de vídeo / Enables video capturing
				RecorderMode = RecorderMode.Video // Define o modo do gravador como vídeo / Sets the recorder mode to video
			}
		};

		// Criação do gravador com as opções definidas / Create the recorder with the specified options
		Recorder rec = Recorder.CreateRecorder(options);

		// Eventos de gravação / Recording events
		rec.OnRecordingComplete += Rec_OnRecordingComplete; // Evento ao completar a gravação / Event on recording completion
		rec.OnRecordingFailed += Rec_OnRecordingFailed; // Evento ao falhar a gravação / Event on recording failure
		rec.OnStatusChanged += Rec_OnStatusChanged; // Evento ao mudar o status da gravação / Event on recording status change

		// Define as opções e inicia a gravação / Set options and start recording
		rec.SetOptions(options);
		rec.Record(videoPath);

		// Exibir o caminho do arquivo no console / Display the file path in the console
		Console.WriteLine($"Recording to: {videoPath}");
		Console.WriteLine("Press any key to exit...");
		Console.ReadKey();

		// Descartar o gravador quando a gravação estiver completa / Dispose the recorder when the recording is complete
		rec.Dispose();
	}

	// Método chamado ao completar a gravação / Method called upon recording completion
	private static void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
	{
		Console.WriteLine($"Recording complete. File saved at: {e.FilePath}");
	}

	// Método chamado ao falhar a gravação / Method called upon recording failure
	private static void Rec_OnRecordingFailed(object sender, RecordingFailedEventArgs e)
	{
		Console.WriteLine($"Recording failed with error: {e.Error}");
	}

	// Método chamado ao mudar o status da gravação / Method called upon status change of the recording
	private static void Rec_OnStatusChanged(object sender, RecordingStatusEventArgs e)
	{
		Console.WriteLine($"Recorder status changed: {e.Status}");
	}
}
