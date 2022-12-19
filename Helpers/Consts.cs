namespace CopyOrExportTaskConsoleApp
{
    public static class Consts
    {
        public const string CopyTask = "copy";
        public const string ExportTask = "nwc";
        public const string IniFilePath = "config.ini";

        public const string CopySettingsSection = "copier";
        public const string CopySettingsTitleKey = "title";
        public const string CopySettingsProjectCodeKey = "project_code";
        public const string CopySettingsDestinationKey = "dest";
        public const string CopySettingsSuffixKey = "suffix";
        public const string CopySettingsLogKey = "log";
        public const string CopyFilesSection = "copier_files";

        public const string NwcSettingsSection = "nwc";
        public const string NwcSettingsVersionKey = "nwc_version";
        public const string NwcSettingsPathKey = "nwc_path";
        public const string NwcSettingsTempKey = "nwc_temp";
        public const string NwcSettingsDestinationKey = "nwc_destination";
        public const string ExportFilesSection = "nwc_files";
    }
}