using IniParser;
using IniParser.Model;
using IniParser.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RfaD_Organizer
{

    public partial class ROrganizer : Form
    {
        private string ModpackDir
        {
            get
            {
                if (!string.IsNullOrEmpty(modpackBrowseBox.Text) && Directory.Exists(modpackBrowseBox.Text))
                    return modpackBrowseBox.Text;
                return null;
            }
        }

        private string SkyrimDir
        {
            get
            {
                if (!string.IsNullOrEmpty(skyrimBrowseBox.Text) && Directory.Exists(skyrimBrowseBox.Text))
                    return skyrimBrowseBox.Text;
                return null;
            }
        }

        private string OrganizerDir
        {
            get
            {
                if (!string.IsNullOrEmpty(organizerBrowseBox.Text) && Directory.Exists(organizerBrowseBox.Text))
                    return organizerBrowseBox.Text;
                return null;
            }
        }

        private string ModpackDataDir
        {
            get
            {
                if (!string.IsNullOrEmpty(ModpackDir) && Directory.Exists(ModpackDir + "\\Data"))
                    return ModpackDir + "\\Data";
                return null;
            }
        }

        private string OrganizerAppDataDir
        {
            get
            {
                string appDataOrganizerDir = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData
                    ) + "\\ModOrganizer\\Skyrim";
                if (Directory.Exists(appDataOrganizerDir))
                    return appDataOrganizerDir;
                return null;
            }
        }

        private string OrganizerIniFileDir
        {
            get
            {
                if (!string.IsNullOrEmpty(OrganizerDir) && File.Exists(OrganizerDir + "\\ModOrganizer.ini"))
                    return OrganizerDir + "\\ModOrganizer.ini";
                else if (!string.IsNullOrEmpty(OrganizerAppDataDir) && File.Exists(OrganizerAppDataDir + "\\ModOrganizer.ini"))
                    return OrganizerAppDataDir + "\\ModOrganizer.ini";
                return null;
            }
        }

        private string OrganizerModsDir
        {
            get
            {
                string modsDir = GetINIValue("Settings", "mods_directory");
                if (string.IsNullOrEmpty(modsDir))
                    modsDir = OrganizerDir + "\\mods";
                return modsDir;
            }
        }

        private string OrganizerProfilesDir
        {
            get
            {
                string profilesDir = GetINIValue("Settings", "profiles_directory");
                if (string.IsNullOrEmpty(profilesDir))
                    profilesDir = OrganizerDir + "\\profiles";
                return profilesDir;
            }
        }

        private IniData organizerIniData = null;

        public ROrganizer()
        {
            InitializeComponent();
            CenterToScreen();
            if (!string.IsNullOrEmpty(OrganizerIniFileDir))
            {
                organizerIniData = ReadINIFile(OrganizerIniFileDir);
                if (organizerIniData != null)
                {
                    string baseDir = Path.GetFullPath(organizerIniData["Settings"]["base_directory"]);
                    if (!string.IsNullOrEmpty(baseDir) && Directory.Exists(baseDir))
                    {
                        organizerBrowseBox.Text = baseDir;
                    }
                }
            }  
        }

        private string GetSkyrimDirectory()
        {
            string gamePath = organizerIniData["General"]["gamePath"];
            if (gamePath.StartsWith("@ByteArray"))
            {
                gamePath = gamePath.Replace("@ByteArray", "");
            }
            char[] parenthesis = { '(', ')' };
            gamePath = gamePath.Trim(parenthesis);
            return Path.GetFullPath(gamePath);
        }

        private void ToggleButtonsIfDirsExists()
        {
            if (!string.IsNullOrEmpty(ModpackDir) && !string.IsNullOrEmpty(OrganizerDir) && !string.IsNullOrEmpty(SkyrimDir))
            {
                doAllButton.Enabled = !doAllButton.Enabled;
                organizeModsButton.Enabled = !organizeModsButton.Enabled;
                addExecutablesButton.Enabled = !addExecutablesButton.Enabled;
                createProfileButton.Enabled = !createProfileButton.Enabled;
            }
        }

        private void modpackBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                modpackBrowseBox.Text = FBD.SelectedPath;
            }
        }

        private void skyrimBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                skyrimBrowseBox.Text = FBD.SelectedPath;
            }
        }

        private void organizerBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                organizerBrowseBox.Text = FBD.SelectedPath;
            }
        }

        private void doAllButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(OrganizerModsDir))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог модов не существует.");
                return;
            }
            if (!Directory.Exists(ModpackDataDir))
            {
                ShowErrorMessage("По указанному пути отсутствует папка Data: \n" + ModpackDataDir);
                return;
            }
            if (!Directory.Exists(OrganizerProfilesDir))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог профилей не существует.");
                return;
            }

            StartWorker();
            AddExecutables();
            CreateProfile();
            MessageBox.Show("Добавлены исполняемые файлы для FNISforUsers и Reqtificator.\n\n" +
                "Создан профиль Requiem for a Dream:\n" + OrganizerProfilesDir + ".\n" +
                "Ожидайте организации модов.",
                "Операция выполнена", MessageBoxButtons.OK);
        }

        private void createProfileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OrganizerDir))
            {
                ShowErrorMessage("Путь к папке Mod Organizer 2 обязателен к заполнению.");
                return;
            }
            if (!Directory.Exists(OrganizerDir))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + OrganizerDir);
                return;
            }
            if (!Directory.Exists(OrganizerProfilesDir))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог профилей не существует.");
                return;
            }

            CreateProfile();
            MessageBox.Show("Создан профиль Requiem for a Dream:\n" + OrganizerProfilesDir,
                "Операция выполнена", MessageBoxButtons.OK);
        }

        private void organizeModsButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OrganizerModsDir))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог модов не существует.");
                return;
            }
            if (string.IsNullOrEmpty(ModpackDataDir))
            {
                ShowErrorMessage("По указанному пути отсутствует папка Data: \n" + ModpackDataDir);
                return;
            }

            StartWorker();
        }

        private void StartWorker()
        {
            modpackBrowseBox.Enabled = false;
            modpackBrowseButton.Enabled = false;
            skyrimBrowseBox.Enabled = false;
            skyrimBrowseButton.Enabled = false;
            organizerBrowseBox.Enabled = false;
            organizerBrowseButton.Enabled = false;
            doAllButton.Visible = false;
            organizeModsButton.Visible = false;
            createProfileButton.Visible = false;
            addExecutablesButton.Visible = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Maximum = 100;
            cancelButton.Visible = true;

            backgroundWorker1.RunWorkerAsync();
        }

        private void addExecutablesButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OrganizerDir) || string.IsNullOrEmpty(SkyrimDir))
            {
                ShowErrorMessage("Поля путей Skyrim и Mod Organizer 2 обязательны к заполнению");
                return;
            }
            if (!Directory.Exists(OrganizerDir))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + OrganizerDir);
                return;
            }
            if (!Directory.Exists(SkyrimDir))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + SkyrimDir);
                return;
            }
            if (!Directory.Exists(OrganizerModsDir))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог модов не существует.");
                return;
            }
            AddExecutables();
            MessageBox.Show("Добавлены исполняемые файлы для FNISforUsers и Reqtificator",
                "Операция выполнена", MessageBoxButtons.OK);
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message,
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private static void ExtractResource(string resDir, string resName, string outDir)
        {
            resDir += "." + resName;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream(resDir))
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDir + "\\" + resName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
                w.Write(r.ReadBytes((int)s.Length));
        }

        private List<string> DirSearch(string sDir, List<string> files = null)
        {
            if (files == null)
                files = new List<string>();
            foreach (string f in Directory.GetFiles(sDir))
                files.Add(f);

            foreach (string d in Directory.GetDirectories(sDir))
                files = DirSearch(d, files);
            return files;
        }

        private IniData ReadINIFile(string iniFileDir)
        {
            IniDataParser _parser = new IniDataParser();
            _parser.Configuration.AssigmentSpacer = "";
            FileIniDataParser parser = new FileIniDataParser(_parser);
            Encoding utf8 = new UTF8Encoding();
            return parser.ReadFile(iniFileDir, utf8);
        }

        private void WriteINIFile(IniData iniFile)
        {
            IniDataParser _parser = new IniDataParser();
            _parser.Configuration.AssigmentSpacer = "";
            FileIniDataParser parser = new FileIniDataParser(_parser);
            Encoding utf8 = new UTF8Encoding();
            parser.WriteFile(OrganizerIniFileDir, iniFile, utf8);
        }

        private bool INIHasExecutable(KeyDataCollection section, string title)
        {
            foreach (KeyData key in section)
                if (key.Value == title)
                    return true;
            return false;
        }

        private void AddCustomExecutable(string title, string binary, string arguments = "",
            string workingDirectory = "", string toolbar = "false", string ownicon = "false")
        {
            IniData iniFile = organizerIniData;
            KeyDataCollection customExecutables = iniFile["customExecutables"];
            if (INIHasExecutable(customExecutables, title))
                return;
            string nextId = (Int16.Parse(customExecutables["size"]) + 1).ToString();
            customExecutables.AddKey(nextId + "\\title", title);
            customExecutables.AddKey(nextId + "\\toolbar", toolbar);
            customExecutables.AddKey(nextId + "\\ownicon", ownicon);
            binary = binary.Replace("\\", "/");
            customExecutables.AddKey(nextId + "\\binary", binary);
            customExecutables.AddKey(nextId + "\\arguments", arguments);
            workingDirectory = workingDirectory.Replace("\\", "/");
            customExecutables.AddKey(nextId + "\\workingDirectory", workingDirectory);
            customExecutables.AddKey(nextId + "\\steamAppID", "");
            customExecutables.AddKey(nextId + "\\hide", "false");
            customExecutables["size"] = nextId;
            WriteINIFile(iniFile);
        }

        private void CreateProfile()
        {
            string profileDir = OrganizerProfilesDir + "\\Requiem for a Dream";
            Directory.CreateDirectory(profileDir);
            List<string> profileRes = new List<string>() { "loadorder.txt", "modlist.txt", "plugins.txt", "settings.ini" };
            foreach (string res in profileRes)
                ExtractResource("RfaD_Organizer.Resources.Profile", res, profileDir);
        }

        private string GetINIValue(string section, string value)
        {
            if (section == "Settings")
            {
                string baseDir = organizerIniData[section]["base_directory"];

                if (!string.IsNullOrEmpty(baseDir))
                    baseDir = baseDir.Replace("/", "\\");
                else
                    baseDir = OrganizerAppDataDir;

                if (value == "base_directory")
                    return baseDir;

                string customDir = organizerIniData[section][value];
                if (customDir != null)
                    customDir = customDir.Replace("%BASE_DIR%", baseDir).Replace("/", "\\");
                return customDir;
            }
            return organizerIniData[section][value];
        }

        private void AddExecutables()
        {
            string fnisGenerateDir = "\\tools\\generatefnis_for_users";
            string fnisDir = OrganizerModsDir + "\\[RfaD] FNIS Behavior" + fnisGenerateDir;
            AddCustomExecutable(title: "[Rfad] GenerateFNISforUsers",
                binary: fnisDir + "\\GenerateFNISforUsers.exe",
                workingDirectory: fnisDir,
                toolbar: "true");
            string fnisMyPatchesDir = OrganizerModsDir + "\\[RfaD] FNISforUsers" + fnisGenerateDir;
            Directory.CreateDirectory(fnisMyPatchesDir);
            ExtractResource("RfaD_Organizer.Resources.Fnis", "MyPatches.txt", fnisMyPatchesDir);
            string reqtificatorSavefileDir = OrganizerModsDir + "\\[RfaD] Reqtificator\\skyproc patchers\\requiem\\Files";
            AddCustomExecutable(title: "[Rfad] Reqtificator",
                binary: Environment.SystemDirectory + "\\cmd.exe",
                arguments: "/K Reqtificator.bat",
                workingDirectory: SkyrimDir + "\\Data",
                toolbar: "true");
            Directory.CreateDirectory(reqtificatorSavefileDir);
            ExtractResource("RfaD_Organizer.Resources.Reqtificator", "Savefile", reqtificatorSavefileDir);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("RfaD_Organizer.Resources.allfiles.json"))
            using (StreamReader r = new StreamReader(stream))
            {
                Dictionary<string, string> skyrimFiles = JsonConvert.DeserializeObject<Dictionary<string, string>>(r.ReadToEnd());
                List<string> dataFiles = DirSearch(ModpackDataDir);
                int progress_max = dataFiles.Count;
                int progress = 0;
                List<string> installedMods = new List<string>();

                //Organize mods. Can be cancelled
                foreach (string dataFile in dataFiles)
                {
                    string src = dataFile;
                    string fileName = Path.GetFileName(dataFile);
                    string subFilePath = Path.GetDirectoryName(dataFile).Substring(ModpackDataDir.Length).TrimStart(Path.DirectorySeparatorChar).ToLower();
                    if (string.IsNullOrEmpty(subFilePath))
                        subFilePath += fileName;
                    else
                        subFilePath += "\\" + fileName;
                    string modName = skyrimFiles[subFilePath];
                    string dstPath = OrganizerModsDir + "\\" + modName + "\\" + Path.GetDirectoryName(subFilePath);
                    string dst = dstPath + "\\" + fileName;
                    Directory.CreateDirectory(dstPath);
                    File.Copy(src, dst, true);
                    progress++;
                    int progressComplete = (int)((float)progress / progress_max * 100);
                    worker.ReportProgress(progressComplete);
                    if (!installedMods.Contains(modName))
                        installedMods.Add(modName);
                    if (worker.CancellationPending)
                    {
                        worker.ReportProgress(-1);
                        foreach (string mod in installedMods)
                        {
                            string modDir = OrganizerModsDir + "\\" + mod;
                            DirectoryInfo modDirInfo = new DirectoryInfo(modDir);
                            foreach (FileInfo file in modDirInfo.EnumerateFiles())
                            {
                                file.Delete();
                            }
                            foreach (DirectoryInfo dir in modDirInfo.EnumerateDirectories())
                            {
                                dir.Delete(true);
                            }
                            Directory.Delete(modDir);
                        }
                        e.Cancel = true;
                        return;
                    }
                }
            }

            //Add dummy mods and mods separators
            List<string> modsDirsToCreate = new List<string>() {
                "[RfaD] Reqtificator",
                "[RfaD] FNISforUsers",
                "UTILS_separator",
                "SKSE_separator",
                "CORE_separator",
                "UI_separator",
                "VISUAL_separator",
                "MODS_separator"
            };
            foreach (string dir in modsDirsToCreate)
                Directory.CreateDirectory(OrganizerModsDir + "\\" + dir);

            //Copy SKSE files to Skyrim root directory
            string[] dllFiles = Directory.GetFiles(OrganizerModsDir, "*.dll");
            string[] exeFiles = Directory.GetFiles(OrganizerModsDir, "*.exe");
            string[] rootFiles = new string[dllFiles.Length + exeFiles.Length];
            dllFiles.CopyTo(rootFiles, 0);
            exeFiles.CopyTo(rootFiles, dllFiles.Length);
            foreach (string file in rootFiles)
            {
                string dstFile = SkyrimDir + "\\" + Path.GetFileName(file);
                File.Copy(file, dstFile, true);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            modpackBrowseBox.Enabled = true;
            modpackBrowseButton.Enabled = true;
            skyrimBrowseBox.Enabled = true;
            skyrimBrowseButton.Enabled = true;
            organizerBrowseBox.Enabled = true;
            organizerBrowseButton.Enabled = true;
            progressBar1.Visible = false;
            cancelButton.Visible = false;
            doAllButton.Visible = true;
            organizeModsButton.Visible = true;
            createProfileButton.Visible = true;
            addExecutablesButton.Visible = true;
            if (e.Cancelled == true)
            {
                MessageBox.Show("Операция отменена\n" +
                    "Организованные моды удалены.", "Отмена операции",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (e.Error != null)
            {
                ShowErrorMessage("Ошибка:" + e.Error.Message);
                return;
            }
            MessageBox.Show("Моды с префиксом [RfaD] организованы в папке:\n"
                + OrganizerModsDir, "Операция выполнена", MessageBoxButtons.OK);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
                progressBar1.Style = ProgressBarStyle.Marquee;
            else
                progressBar1.Value = e.ProgressPercentage;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }

        private void organizerBrowseBox_TextChanged(object sender, EventArgs e)
        {
            organizerIniData = ReadINIFile(OrganizerIniFileDir);
            skyrimBrowseBox.Text = GetSkyrimDirectory();
            ToggleButtonsIfDirsExists();
        }

        private void modpackBrowseBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButtonsIfDirsExists();
        }

        private void skyrimBrowseBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButtonsIfDirsExists();
        }
    }
}
