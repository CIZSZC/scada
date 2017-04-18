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
 * Summary  : Image output condition
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Drawing.Design;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Image output condition
    /// <para>Условие вывода изображения</para>
    /// </summary>
    public class Condition : ISchemeDocAvailable
    {
        /// <summary>
        /// Наименование категории условия
        /// </summary>
        protected const string ConditionCat = "Condition";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Condition()
        {
            CompareOperator1 = CompareOperators.LessThan;
            CompareArgument1 = 0.0;
            LogicalOperator = LogicalOperators.None;
            CompareOperator2 = CompareOperators.LessThan;
            CompareArgument2 = 0.0;
            ImageName = "";
            SchemeDoc = null;
        }


        /// <summary>
        /// Получить или установить 1-й оператор сравнения
        /// </summary>
        #region Attributes
        [DisplayName("Compare oper. 1"), Category(ConditionCat)]
        #endregion
        public CompareOperators CompareOperator1 { get; set; }

        /// <summary>
        /// Получить или установить аргумент для сравнения 1-м оператором
        /// </summary>
        #region Attributes
        [DisplayName("Argument 1"), Category(ConditionCat)]
        [CM.DefaultValue(0.0)]
        #endregion
        public double CompareArgument1 { get; set; }

        /// <summary>
        /// Получить или установить логический оператор, применяемый к результатам сравнения
        /// </summary>
        #region Attributes
        [DisplayName("Logical oper."), Category(ConditionCat)]
        [CM.DefaultValue(LogicalOperators.None)]
        #endregion
        public LogicalOperators LogicalOperator { get; set; }

        /// <summary>
        /// Получить или установить 2-й оператор сравнения
        /// </summary>
        #region Attributes
        [DisplayName("Compare oper. 2"), Category(ConditionCat)]
        #endregion
        public CompareOperators CompareOperator2 { get; set; }

        /// <summary>
        /// Получить или установить аргумент для сравнения 2-м оператором
        /// </summary>
        #region Attributes
        [DisplayName("Argument 2"), Category(ConditionCat)]
        [CM.DefaultValue(0.0)]
        #endregion
        public double CompareArgument2 { get; set; }

        /// <summary>
        /// Получить или установить наименование изображения, отображаемого при выполнении условия
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        [CM.TypeConverter(typeof(ImageConverter)), CM.Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [CM.DefaultValue("")]
        #endregion
        public string ImageName { get; set; }

        /// <summary>
        /// Получить или установить ссылку на свойства документа схемы
        /// </summary>
        [CM.Browsable(false), ScriptIgnore]
        public SchemeDocument SchemeDoc { get; set; }


        /// <summary>
        /// Преобразовать оператор сравнения в строку
        /// </summary>
        private string OperToString(CompareOperators oper)
        {
            switch (oper)
            {
                case CompareOperators.Equal:
                    return " = ";
                case CompareOperators.NotEqual:
                    return " <> ";
                case CompareOperators.LessThan:
                    return " < ";
                case CompareOperators.LessThanEqual:
                    return " <= ";
                case CompareOperators.GreaterThan:
                    return " > ";
                default: // CompareOperators.GreaterThanEqual
                    return " >= ";
            }
        }

        /// <summary>
        /// Преобразовать логический оператор в строку
        /// </summary>
        private string OperToString(LogicalOperators oper)
        {
            switch (oper)
            {
                case LogicalOperators.And:
                    return Localization.UseRussian ? " И " : " And ";
                case LogicalOperators.Or:
                    return Localization.UseRussian ? " Или " : " Or ";
                default: // LogicalOperators.None
                    return "";
            }
        }


        /// <summary>
        /// Загрузить условие из XML-узла
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            CompareOperator1 = xmlNode.GetChildAsEnum<CompareOperators>("CompareOperator1");
            CompareArgument1 = xmlNode.GetChildAsDouble("CompareArgument1");
            CompareOperator2 = xmlNode.GetChildAsEnum<CompareOperators>("CompareOperator2");
            CompareArgument2 = xmlNode.GetChildAsDouble("CompareArgument2");
            LogicalOperator = xmlNode.GetChildAsEnum<LogicalOperators>("LogicalOperator");
            ImageName = xmlNode.GetChildAsString("ImageName");
        }

        /// <summary>
        /// Клонировать объект
        /// </summary>
        public Condition Clone()
        {
            return new Condition()
            {
                CompareOperator1 = CompareOperator1,
                CompareArgument1 = CompareArgument1,
                LogicalOperator = LogicalOperator,
                CompareOperator2 = CompareOperator2,
                CompareArgument2 = CompareArgument2,
                ImageName = ImageName,
                SchemeDoc = SchemeDoc
            };
        }

        /// <summary>
        /// Получить строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Localization.UseRussian ? "Знач." : "Value");
            sb.Append(OperToString(CompareOperator1));
            sb.Append(CompareArgument1);
            if (LogicalOperator != LogicalOperators.None)
            {
                sb.Append(OperToString(LogicalOperator));
                sb.Append(Localization.UseRussian ? "Знач." : "Value");
                sb.Append(OperToString(CompareOperator2));
                sb.Append(CompareArgument2);
            }
            return sb.ToString();
        }
    }
}
