﻿/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2015 University of Washington - Seattle, WA
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using pwiz.Common.Collections;

namespace pwiz.Common.SystemUtil
{
    public static class FormUtil
    {
        /// <summary>
        /// Shows a dialog box.
        /// If the owner of the dialog is a popup window, then this method uses <see cref="FormUtil.FindTopLevelOwner"/> 
        /// to find the appropriate main window to own the dialog, and after the dialog is closed, sets the focus back 
        /// to the correct control.
        /// </summary>
        public static DialogResult ShowDialog(Control owner, Form dialog)
        {
            Form ownerForm = null;
            if (null != owner)
            {
                ownerForm = owner.FindForm();
            }
            var topLevelOwner = FindTopLevelOwner(owner);
            Control activeControl = null;
            if (null != ownerForm && ownerForm.ContainsFocus)
            {
                activeControl = ownerForm.ActiveControl;
            }
            var dialogResult = dialog.ShowDialog(topLevelOwner);
            if (null != activeControl)
            {
                if (ownerForm != topLevelOwner)
                {
                    // Put the focus first on the window which was the owner of the dialog box.
                    // Otherwise when the ownerForm is closed, the focus will go to a different application
                    topLevelOwner.Focus();
                    // Then put the focus on the control which had the focus before the dialog came up
                    activeControl.Focus();
                }
            }
            return dialogResult;
        }

        /// <summary>
        /// Moves the control's X and Y coordinates according to the X and Y values.
        /// </summary>
        public static void Offset(this Control control, int x = 0, int y = 0)
        {
            var loc = control.Location;
            loc.Offset(x, y);
            control.Location = loc;
        }

        /// <summary>
        /// Returns a point with its X and Y coordinates offset according to the given X and Y values.
        /// </summary>
        public static System.Drawing.Point Offset(this System.Drawing.Point point, int x = 0, int y = 0)
        {
            point.Offset(x, y);
            return point;
        }

        /// <summary>
        /// Finds the top level form which is suitable to pass to <see cref="Form.ShowDialog(IWin32Window)"/>.
        /// This function looks for a form for which ShowInTaskBar is true.  When dialogs are shown that are owned
        /// by a popup form which is not ShowInTaskBar, it often prevents the user from Alt-Tabbing back to 
        /// the application.
        /// </summary>
        public static Control FindTopLevelOwner(Control control)
        {
            if (null == control)
            {
                return null;
            }
            var topLevelForm = control.TopLevelControl as Form;
            if (null == topLevelForm)
            {
                return control;
            }
            if (IsSuitableDialogOwner(topLevelForm))
            {
                return topLevelForm;
            }
            for (var formOwner = topLevelForm.Owner; null != formOwner; formOwner = formOwner.Owner)
            {
                if (IsSuitableDialogOwner(formOwner))
                {
                    return formOwner;
                }
            }
            return topLevelForm;
        }

        public static bool IsSuitableDialogOwner(Form form)
        {
            return form.ShowInTaskbar || form.Modal;
        }

        public static Form FindTopLevelOpenForm(Func<Form, bool> skipForm = null)
        {
            Form[] openForms = OpenForms;
            for (int i = openForms.Length - 1; i >= 0; i--)
            {
                Form form = openForms[i];
                if (skipForm != null && skipForm(form))
                    continue;
                if (form.IsDisposed)
                    continue;
                return form;
            }
            // Should never happen
            return null;
        }

        public static T FindParentOfType<T>(Control control) where T : class
        {
            while (control != null && !(control is T))
                control = control.Parent;
            return control as T;
        }

        /// <summary>
        /// Returns all open forms in the application.
        /// Thread-safe version of <see cref="Application.OpenForms"/>.
        /// </summary>
        public static Form[] OpenForms
        {
            get
            {
                while (true)
                {
                    try
                    {
                        return Application.OpenForms.OfType<Form>().ToArray();
                    }
                    catch (InvalidOperationException)
                    {
                        // Collection was modified. Try again.
                    }
                }
            }
        }
        /// <summary>
        /// Set the tooltips for the control and all of its children to null.
        /// The ToolTip control sometimes gets confused if any of the tooltips belong to
        /// controls that are no longer part of the form.
        /// (ToolTip.TopLevelControl sometimes gets set to a bogus value)
        /// </summary>
        public static void PurgeTooltips(IList<ToolTip> toolTipControls, Control control)
        {
            foreach (var toolTipControl in toolTipControls)
            {
                toolTipControl.SetToolTip(control, null);
            }

            foreach (var child in control.Controls.OfType<Control>())
            {
                PurgeTooltips(toolTipControls, child);
            }
        }

        /// <summary>
        /// Removes the tab page, and nulls out the tooltips that may have been set.
        /// </summary>
        public static void RemoveTabPage(TabPage tabPage, IList<ToolTip> toolTipControls)
        {
            ((TabControl) tabPage.Parent).TabPages.Remove(tabPage);
            if (toolTipControls != null && toolTipControls.Count > 0)
            {
                PurgeTooltips(toolTipControls, tabPage);
            }
        }

        public static void RemoveTabPage(TabPage tabPage, ToolTip toolTipControl)
        {
            RemoveTabPage(tabPage, ImmutableList.Singleton(toolTipControl));
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void SetForegroundWindow(this Control control)
        {
            SetForegroundWindow(control.Handle);
        }

        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public static void HideCaret(this Control control)
        {
            HideCaret(control.Handle);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        public static void SetScrollPos(this Control control, Orientation sd, int pos)
        {
            SetScrollPos(control.Handle, (int)sd, pos, true);
        }

        public static Control GetFocus(this Control control)
        {
            if (control.Focused)
                return control;
            return (
                from Control childControl in control.Controls
                select GetFocus(childControl)).FirstOrDefault(focus => focus != null);
        }

        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_SHOWWINDOW = 0x0040;

        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static void BringWindowToSameLevelWithoutActivating(this Form targetWindow, IntPtr referenceWindowHandle)
        {
            // Use SetWindowPos to adjust z-order without activating
            SetWindowPos(targetWindow.Handle, referenceWindowHandle, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }
    }
}
