﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using zoft.MauiExtensions.Controls;

namespace viwik.CantoneseLearning.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

                builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()        
                .UseZoftAutoCompleteEntry()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FASolid");
                    fonts.AddFont("Font Awesome 7 Free-Regular-400.otf", "FARegular");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
