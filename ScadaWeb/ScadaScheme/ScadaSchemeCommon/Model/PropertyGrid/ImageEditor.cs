﻿/*
 * Copyright 2017 Mikhail Shiryaev
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
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaSchemeCommon
 * Summary  : Editor of images for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

 #pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Editor of images for PropertyGrid
    /// <para>Редактор изображений для PropertyGrid</para>
    /// </summary>
    public class ImageEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorSvc = provider == null ? null :
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null && context.Instance != null && editorSvc != null)
            {
                Type propType = context.PropertyDescriptor.PropertyType;

                if (propType == typeof(Dictionary<string, Image>) && context.Instance is SchemeDocument)
                {
                    // редактирование словаря изображений
                    Dictionary<string, Image> images = (Dictionary<string, Image>)value;
                    SchemeDocument schemeDoc = (SchemeDocument)context.Instance;
                    FrmImageDialog frmImageDialog = new FrmImageDialog(images, schemeDoc);
                    editorSvc.ShowDialog(frmImageDialog);
                }
                else if (propType == typeof(string) && context.Instance is ISchemeDocAvailable)
                {
                    // выбор изображения
                    string imageName = (value ?? "").ToString();
                    SchemeDocument schemeDoc = ((ISchemeDocAvailable)context.Instance).SchemeDoc;
                    FrmImageDialog frmImageDialog = new FrmImageDialog(imageName, schemeDoc.Images, schemeDoc);

                    if (editorSvc.ShowDialog(frmImageDialog) == DialogResult.OK)
                        value = frmImageDialog.SelectedImageName;
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
