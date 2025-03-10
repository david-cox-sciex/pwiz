﻿using System;
using System.Linq;
using System.Windows.Forms;
using SharedBatch.Properties;

namespace SharedBatch
{
    public enum PathDialogOptions
    {
        File,  // the desired path is a file path
        Folder,  // the desired path is a folder path
        Save,  // use a save file dialog
        ExistingOptional  // do not check if file exists
    }

    public partial class FilePathControl : UserControl, IValidatorControl
    {
        // A control used by the InvalidConfigSetupForm to correct invalid file/folder paths

        // Implements IValidatorControl:
        //    - GetVariable() returns the current path (_path)
        //    - IsValid() uses the pathValidator to determine if _path is valid

        private string _path; // the current path displayed in the textFilePath TextBox
        private string _lastUsedPath; // the last path the user navigated to in a open File or open folder dialog
        private readonly string _filter; // the filter to use in a OpenFileDialog. Has no impact when _folder == true.
        private readonly PathDialogOptions[] _pathDialogOptions;
        private readonly Validator _pathValidator; // the validator to use on the path. Throws an ArgumentException if the path is invalid.
        
        public FilePathControl(string variableName, string invalidPath, string lastInputPath, Validator pathValidator, string filter, params PathDialogOptions[] dialogOptions)
        {
            InitializeComponent();
            _path = invalidPath;
            _lastUsedPath = lastInputPath ?? invalidPath;
            _pathValidator = pathValidator;
            _pathDialogOptions = dialogOptions;
            if (_pathDialogOptions.Contains(PathDialogOptions.File))
                _filter = filter;
            label1.Text = string.Format(Resources.FilePathControl_FilePathControl_Could_not_find_the__0__, variableName);
            label2.Text = string.Format(Resources.FilePathControl_FilePathControl_Please_correct_the__0__to_continue_, variableName);
            textFilePath.Text = _path;
            textFilePath.TextChanged += textFilePath_TextChanged;
        }

        public object GetVariable() => _path;

        public bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            try
            {
                _pathValidator(_path);
                return true;
            } catch (ArgumentException e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var initialDirectory = FileUtil.GetInitialDirectory(_lastUsedPath);
            if (_pathDialogOptions.Contains(PathDialogOptions.Folder))
            {
                var folderPath = UiFileUtil.OpenFolder(initialDirectory);
                if (folderPath != null)
                    textFilePath.Text = folderPath;
            }
            else
            {
                var saveFileDialog = _pathDialogOptions.Contains(PathDialogOptions.Save);
                FileDialog dialog = saveFileDialog ? (FileDialog)new SaveFileDialog() : new OpenFileDialog();
                dialog.CheckFileExists = !saveFileDialog && !_pathDialogOptions.Contains(PathDialogOptions.ExistingOptional);
                dialog.Filter = _filter;
                dialog.InitialDirectory = initialDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textFilePath.Text = dialog.FileName;
            }
        }

        private void textFilePath_TextChanged(object sender, EventArgs e)
        {
            _path = textFilePath.Text;
            _lastUsedPath = textFilePath.Text;
        }

        public void SetInput(object variable)
        {
            textFilePath.Text = (string)variable;
        }
    }
}
