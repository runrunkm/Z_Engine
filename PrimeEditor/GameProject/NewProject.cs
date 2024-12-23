﻿using PrimeEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrimeEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType {  get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }

        public string IconFilePath {  get; set; }
        public string ScreenshotFilePath { get; set; }
        public string ProjectFilePath {  get; set; }

    }

    class NewProject : ViewModeBase
    {
        private readonly string _templatePath = @"..\..\..\PrimeEditor\ProjectTemplates";
        private string _Projectname = "NewProject";
        public string ProjectName
        {
            get { return _Projectname; }

            set 
            {
                if (_Projectname != value)
                {
                    _Projectname = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _Projectpath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\PrimeProject\";
        public string ProjectPath
        {
            get { return _Projectpath; }

            set
            {
                if (_Projectpath != value)
                {
                    _Projectpath = value;
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
        
        public NewProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            try
            {   
                var templatesFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templatesFiles.Any());
                foreach (var file in templatesFiles)
                {
                    var template = Serializer.FromFile<ProjectTemplate>(file);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Screenshot.png"));
                    template.Screenshot = File.ReadAllBytes(template.ScreenshotFilePath);
                    _projectTemplates.Add(template);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
    }


}
