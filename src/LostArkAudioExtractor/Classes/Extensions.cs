namespace LostArkAudioExtractor.Classes {

    internal static class Extensions {

        public static ExportType ToExportType(this string type) {
            if (string.IsNullOrEmpty(type)) 
                return ExportType.Unknown;
            switch (type.ToLower()) {
                case "m":  return ExportType.MP3;
                case "o":  return ExportType.OGG;
                default: return ExportType.Unknown;
            }
        }

    }

}