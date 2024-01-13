using System;
using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ReactiveUI;

namespace PdfViewer.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string? _filePath;
    public string? FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }
    public ReactiveCommand<Unit, Unit> CommandOpenFile { get; set; }

    public MainViewModel()
    {
        CommandOpenFile = ReactiveCommand.CreateFromTask(async () =>
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");
            
            var files = await provider.OpenFilePickerAsync(
                new FilePickerOpenOptions()
                {
                    Title = "Открыть PDF",
                    AllowMultiple = false,
                    FileTypeFilter = new []
                    {
                        new FilePickerFileType("pdf")
                        {
                            Patterns = new [] { "*.pdf" }
                        }
                    }
                });
            FilePath = files[0].Path.ToString();
        });
    }
}