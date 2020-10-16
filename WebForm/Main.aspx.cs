using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileChecker
{
    public partial class Main : Page
    {
        private readonly string saveFile = "~/App_Data/States.txt"; // zde se ulozi historie adresare
        private string servermapped;
        private DirectoryInfo di;
        private FileModel[] oldStates = Array.Empty<FileModel>();
        private bool readyToAnalyze;
        private string[] savedLines;
        private string selectedDirectory;

        public long TotalSize { get; set; }
        private readonly List<FileModel> currentState = new List<FileModel>(100);
        private readonly List<FileModel> newFiles = new List<FileModel>(100);
        private readonly List<FileModel> modifiedFiles = new List<FileModel>(100); // změnou se rozumí změna obsahu daného souboru
        private readonly List<FileModel> deletedFiles= new List<FileModel>(100);

        protected void Page_Load(object sender, EventArgs e)
        {
            servermapped ??= Server.MapPath(saveFile);
            if (File.Exists(servermapped))
            {
                savedLines = File.ReadAllLines(servermapped);
                selectedDirectory = savedLines[0];
                Label2.Text = $"Sledujeme: {selectedDirectory}";
                var fileModels = new FileModel[savedLines.Length - 1];
                for (int i = 1; i <= fileModels.Length; i++)
                {
                    string[] pole = savedLines[i].Split(';');

                    fileModels[i - 1] = new FileModel
                    {
                        Name = pole[0],
                        Size = long.Parse(pole[1]),
                        WriteTime = new DateTime().AddTicks(long.Parse(pole[2]))
                    };
                }

                oldStates = fileModels;
            }
            else
                Label1.Text = "First start";
        }

        public ICollection<FileModel> GetGridData()
        {
            return currentState;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (TextBox1.Text.Length == 0)
            {
                Label1.Text = "Enter Directory";
            }
            else
            {
                try
                {
                    di = new DirectoryInfo(Server.MapPath(TextBox1.Text));
                    if (di.Exists)
                    {
                        try
                        {
                            // kontrola pristupnosti
                            DirectorySecurity accessControlList = di.GetAccessControl();
                            if (selectedDirectory != TextBox1.Text)
                            {
                                // zaciname znovu
                                File.WriteAllText(servermapped, TextBox1.Text);
                                Page_Load(this, null);
                            }

                            selectedDirectory = TextBox1.Text;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Label1.Text = "Insufficient Permissions";

                            return;
                        }

                        readyToAnalyze = true;
                    }
                    else
                    {
                        Label1.Text = "Invalid Directory";
                    }
                }
                catch(HttpException ex)
                {
                    Label1.Text = "Unreachable Directory?" + ex.Message;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (readyToAnalyze || selectedDirectory != null)
            {
                TextBox1.Text = string.Empty;
                uint i = 0;

                di ??= new DirectoryInfo(Server.MapPath(selectedDirectory));
                foreach (FileInfo info in di.GetFiles())
                {
                    if (i == 100)
                        return;

                    var model = new FileModel
                    {
                        FileID = $"{i++}",
                        Name = info.Name,
                        Size = info.Length,
                        WriteTime = info.LastWriteTime
                    };

                    currentState.Add(model);
                    TotalSize += model.Size;
                }

                foreach (FileModel model in oldStates)
                {
                    if (!currentState.Any(x => x.Name == model.Name))
                    {
                        deletedFiles.Add(model);
                        GridDeleted.DataSource = deletedFiles;
                    }
                }

                GridDeleted.DataSource = deletedFiles;
                GridDeleted.DataBind();

                foreach (FileModel model in currentState)
                {
                    if (!oldStates.Any(x => x.Name == model.Name))
                    {
                        newFiles.Add(model);
                        GridNewFiles.DataSource = newFiles;
                    }
                    else
                    {
                        var stary = oldStates.Last(x => x.Name == model.Name);
                        if (stary != null)
                        {
                            if (stary.WriteTime != model.WriteTime)
                                modifiedFiles.Add(model);
                        }
                    }
                }

                GridNewFiles.DataSource = newFiles;
                GridNewFiles.DataBind();

                foreach (FileModel model in modifiedFiles)
                {
                    model.Version = oldStates.Where(x => x.Name == model.Name).Count();
                }

                GridModed.DataSource = modifiedFiles;
                GridModed.DataBind();

                SaveState(newFiles, modifiedFiles, deletedFiles);
            }
        }

        private void SaveState(List<FileModel> newFiles, List<FileModel> modifiedFiles, List<FileModel> deletedFiles)
        {
            string state = string.Empty;
            if (savedLines == null)
            {
                // ukladame poprve
                File.WriteAllText(servermapped, selectedDirectory);
            }
            else
            {
                if (deletedFiles.Count > 0)
                {
                    foreach (FileModel model in deletedFiles)
                    {
                        savedLines = savedLines.Where(x => !x.StartsWith(model.Name)).ToArray();
                    }

                    File.WriteAllLines(servermapped, savedLines);
                }
            }            

            foreach (FileModel model in modifiedFiles)
            {
                state += $"{Environment.NewLine}{model.Name};{model.Size};{model.WriteTime.Ticks}";
            }

            foreach (FileModel model in newFiles)
            {
                state += $"{Environment.NewLine}{model.Name};{model.Size};{model.WriteTime.Ticks}";
            }

            if (state.Length > 0)
                File.AppendAllText(servermapped, state);
        }
    }
}