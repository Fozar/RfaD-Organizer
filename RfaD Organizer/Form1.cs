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
        private string ModpackPath
        {
            get
            {
                if (!string.IsNullOrEmpty(modpackBrowseBox.Text))
                    return modpackBrowseBox.Text;
                return null;
            }
        }

        private string ModpackDataPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ModpackPath))
                    return ModpackPath + "\\Data";
                return null;
            }
        }

        private string SkyrimPath
        {
            get
            {
                if (!string.IsNullOrEmpty(skyrimBrowseBox.Text))
                    return skyrimBrowseBox.Text;
                return null;
            }
        }

        private string MOPath
        {
            get
            {
                if (!string.IsNullOrEmpty(moBrowseBox.Text))
                    return moBrowseBox.Text;
                return null;
            }
        }

        private string MOAppDataPath
        {
            get
            {
                string appDataMODir = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData
                    ) + "\\ModOrganizer\\Skyrim";
                if (Directory.Exists(appDataMODir))
                    return appDataMODir;
                return null;
            }
        }

        private string MOINIFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(MOPath) && File.Exists(MOPath + "\\ModOrganizer.ini"))
                    return MOPath + "\\ModOrganizer.ini";
                else if (!string.IsNullOrEmpty(MOAppDataPath) && File.Exists(MOAppDataPath + "\\ModOrganizer.ini"))
                    return MOAppDataPath + "\\ModOrganizer.ini";
                return null;
            }
        }

        private string MOBasePath
        {
            get
            {
                string basePath = GetINIValue("Settings", "base_directory");
                if (string.IsNullOrEmpty(basePath))
                    basePath = MOPath;
                return basePath;
            }
        }

        private string MOModsPath
        {
            get
            {
                string modsPath = GetINIValue("Settings", "mods_directory");
                if (string.IsNullOrEmpty(modsPath))
                    modsPath = MOPath + "\\mods";
                return modsPath;
            }
        }

        private string MOProfilesPath
        {
            get
            {
                string profilesPath = GetINIValue("Settings", "profiles_directory");
                if (string.IsNullOrEmpty(profilesPath))
                    profilesPath = MOPath + "\\profiles";
                return profilesPath;
            }
        }

        public ROrganizer()
        {
            InitializeComponent();
            CenterToScreen();
            if (MOBasePath != null)
            {
                moBrowseBox.Text = MOBasePath;
                skyrimBrowseBox.Text = GetSkyrimDirectory();
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

        private void moBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                moBrowseBox.Text = FBD.SelectedPath;
            }
        }

        private void doAllButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                ShowErrorMessage("Операция уже выполняется");
                return;
            }
            if (string.IsNullOrEmpty(ModpackPath) || string.IsNullOrEmpty(MOPath) || string.IsNullOrEmpty(SkyrimPath))
            {
                ShowErrorMessage("Все поля обязательны к заполнению");
                return;
            }
            if (!Directory.Exists(ModpackPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + ModpackPath);
                return;
            }
            if (!Directory.Exists(MOPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + MOPath);
                return;
            }
            if (!Directory.Exists(SkyrimPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + SkyrimPath);
                return;
            }
            if (!Directory.Exists(MOModsPath))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог модов не существует.");
                return;
            }
            if (!Directory.Exists(ModpackDataPath))
            {
                ShowErrorMessage("По указанному пути отсутствует папка Data: \n" + ModpackDataPath);
                return;
            }
            if (!Directory.Exists(MOProfilesPath))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог профилей не существует.");
                return;
            }

            modpackBrowseBox.Enabled = false;
            modpackBrowseButton.Enabled = false;
            skyrimBrowseBox.Enabled = false;
            skyrimBrowseButton.Enabled = false;
            moBrowseBox.Enabled = false;
            moBrowseButton.Enabled = false;
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
            AddExecutables();
            CreateProfile();
            MessageBox.Show("Добавлены исполняемые файлы для FNISforUsers и Reqtificator.\n\n" +
                "Создан профиль Requiem for a Dream:\n" + MOProfilesPath + ".\n" +
                "Ожидайте организации модов.",
                "Операция выполнена", MessageBoxButtons.OK);
        }

        private void createProfileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MOPath))
            {
                ShowErrorMessage("Путь к папке Mod Organizer 2 обязателен к заполнению.");
                return;
            }
            if (!Directory.Exists(MOPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + MOPath);
                return;
            }
            if (!Directory.Exists(MOProfilesPath))
            {
                ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог профилей не существует.");
                return;
            }

            CreateProfile();
            MessageBox.Show("Создан профиль Requiem for a Dream:\n" + MOProfilesPath,
                "Операция выполнена", MessageBoxButtons.OK);
        }

        private void organizeModsButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                if (!string.IsNullOrEmpty(ModpackPath) && !string.IsNullOrEmpty(MOPath) && !string.IsNullOrEmpty(SkyrimPath))
                {
                    if (!Directory.Exists(ModpackPath))
                    {
                        ShowErrorMessage("Указанного пути не существует:\n" + ModpackPath);
                        return;
                    }
                    if (!Directory.Exists(MOPath))
                    {
                        ShowErrorMessage("Указанного пути не существует:\n" + MOPath);
                        return;
                    }
                    if (!Directory.Exists(SkyrimPath))
                    {
                        ShowErrorMessage("Указанного пути не существует:\n" + SkyrimPath);
                        return;
                    }
                    if (!Directory.Exists(MOModsPath))
                    {
                        ShowErrorMessage("Не найден файл конфигурации Mod Organizer или каталог модов не существует.");
                        return;
                    }
                    if (!Directory.Exists(ModpackDataPath))
                    {
                        ShowErrorMessage("По указанному пути отсутствует папка Data: \n" + ModpackDataPath);
                        return;
                    }

                    modpackBrowseBox.Enabled = false;
                    modpackBrowseButton.Enabled = false;
                    skyrimBrowseBox.Enabled = false;
                    skyrimBrowseButton.Enabled = false;
                    moBrowseBox.Enabled = false;
                    moBrowseButton.Enabled = false;
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
                else
                {
                    ShowErrorMessage("Все поля обязательны к заполнению");
                }
            }
            else
            {
                ShowErrorMessage("Операция уже выполняется");
            }
        }

        private void addExecutablesButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MOPath) || string.IsNullOrEmpty(SkyrimPath))
            {
                ShowErrorMessage("Поля путей Skyrim и Mod Organizer 2 обязательны к заполнению");
                return;
            }
            if (!Directory.Exists(MOPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + MOPath);
                return;
            }
            if (!Directory.Exists(SkyrimPath))
            {
                ShowErrorMessage("Указанного пути не существует:\n" + SkyrimPath);
                return;
            }
            if (!Directory.Exists(MOModsPath))
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

        private static void ExtractResource(string resPath, string resName, string outDir)
        {
            resPath += "." + resName;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream(resPath))
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

        private IniData ReadINIFile()
        {
            IniDataParser _parser = new IniDataParser();
            _parser.Configuration.AssigmentSpacer = "";
            FileIniDataParser parser = new FileIniDataParser(_parser);
            Encoding utf8 = new UTF8Encoding();
            return parser.ReadFile(MOINIFilePath, utf8);
        }

        private void WriteINIFile(IniData iniFile)
        {
            IniDataParser _parser = new IniDataParser();
            _parser.Configuration.AssigmentSpacer = "";
            FileIniDataParser parser = new FileIniDataParser(_parser);
            Encoding utf8 = new UTF8Encoding();
            parser.WriteFile(MOINIFilePath, iniFile, utf8);
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
            IniData iniFile = ReadINIFile();
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
            string profilePath = MOProfilesPath + "\\Requiem for a Dream";
            Directory.CreateDirectory(profilePath);
            List<string> profileRes = new List<string>() { "loadorder.txt", "modlist.txt", "plugins.txt", "settings.ini" };
            foreach (string res in profileRes)
                ExtractResource("RfaD_Organizer.Resources.Profile", res, profilePath);
        }

        private string GetSkyrimDirectory()
        {
            IniData iniFile = ReadINIFile();
            KeyDataCollection customExecutables = iniFile["customExecutables"];
            string skyrimNum = null;
            foreach (KeyData key in customExecutables)
                if (key.Value == "Skyrim")
                {
                    skyrimNum = key.KeyName.Substring(0, 1);
                    break;
                }
            if (!string.IsNullOrEmpty(skyrimNum))
            {
                string skyrimDir = customExecutables[skyrimNum + "\\workingDirectory"];
                if (!string.IsNullOrEmpty(skyrimDir))
                    return skyrimDir.Replace("/", "\\"); ;
            }
            return null;
        }

        private string GetINIValue(string section, string value)
        {
            IniData iniFile = ReadINIFile();
            if (section == "Settings")
            {
                string baseDir = iniFile[section]["base_directory"];

                if (!string.IsNullOrEmpty(baseDir))
                    baseDir = baseDir.Replace("/", "\\");
                else
                    baseDir = MOAppDataPath;

                if (value == "base_directory")
                    return baseDir;

                string customPath = iniFile[section][value];
                if (customPath != null)
                    customPath = customPath.Replace("%BASE_DIR%", baseDir).Replace("/", "\\");
                return customPath;
            }
            return iniFile[section][value];
        }

        private void AddExecutables()
        {
            string fnisGeneratePath = "\\tools\\generatefnis_for_users";
            string fnisPath = MOModsPath + "\\[RfaD] FNIS Behavior" + fnisGeneratePath;
            AddCustomExecutable(title: "[Rfad] GenerateFNISforUsers",
                binary: fnisPath + "\\GenerateFNISforUsers.exe",
                workingDirectory: fnisPath,
                toolbar: "true");
            string fnisMyPatchesPath = MOModsPath + "\\[RfaD] FNISforUsers" + fnisGeneratePath;
            Directory.CreateDirectory(fnisMyPatchesPath);
            ExtractResource("RfaD_Organizer.Resources.Fnis", "MyPatches.txt", fnisMyPatchesPath);
            string reqtificatorSavefilePath = MOModsPath + "\\[RfaD] Reqtificator\\skyproc patchers\\requiem\\Files";
            AddCustomExecutable(title: "[Rfad] Reqtificator",
                binary: Environment.SystemDirectory + "\\cmd.exe",
                arguments: "/K Reqtificator.bat",
                workingDirectory: SkyrimPath + "\\Data",
                toolbar: "true");
            Directory.CreateDirectory(reqtificatorSavefilePath);
            ExtractResource("RfaD_Organizer.Resources.Reqtificator", "Savefile", reqtificatorSavefilePath);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("RfaD_Organizer.Resources.allfiles.json"))
            using (StreamReader r = new StreamReader(stream))
            {
                Dictionary<string, string> skyrimFiles = JsonConvert.DeserializeObject<Dictionary<string, string>>(r.ReadToEnd());
                List<string> dataFiles = DirSearch(ModpackDataPath);
                int progress_max = dataFiles.Count;
                int progress = 0;
                List<string> installedMods = new List<string>();

                //Organize mods. Can be cancelled
                foreach (string dataFile in dataFiles)
                {
                    string src = dataFile;
                    string fileName = Path.GetFileName(dataFile);
                    string subFilePath = Path.GetDirectoryName(dataFile).Substring(ModpackDataPath.Length).TrimStart(Path.DirectorySeparatorChar).ToLower();
                    if (string.IsNullOrEmpty(subFilePath))
                        subFilePath += fileName;
                    else
                        subFilePath += "\\" + fileName;
                    string modName = skyrimFiles[subFilePath];
                    string dstPath = MOModsPath + "\\" + modName + "\\" + Path.GetDirectoryName(subFilePath);
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
                            string modDir = MOModsPath + "\\" + mod;
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
                Directory.CreateDirectory(MOModsPath + "\\" + dir);

            //Copy SKSE files to Skyrim root directory
            string[] dllFiles = Directory.GetFiles(MOModsPath, "*.dll");
            string[] exeFiles = Directory.GetFiles(MOModsPath, "*.exe");
            string[] rootFiles = new string[dllFiles.Length + exeFiles.Length];
            dllFiles.CopyTo(rootFiles, 0);
            exeFiles.CopyTo(rootFiles, dllFiles.Length);
            foreach (string file in rootFiles)
            {
                string dstFile = SkyrimPath + "\\" + Path.GetFileName(file);
                File.Copy(file, dstFile, true);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            modpackBrowseBox.Enabled = true;
            modpackBrowseButton.Enabled = true;
            skyrimBrowseBox.Enabled = true;
            skyrimBrowseButton.Enabled = true;
            moBrowseBox.Enabled = true;
            moBrowseButton.Enabled = true;
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
                + MOModsPath, "Операция выполнена", MessageBoxButtons.OK);
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

        private void moBrowseBox_TextChanged(object sender, EventArgs e)
        {
            skyrimBrowseBox.Text = GetSkyrimDirectory();
        }
    }
}
