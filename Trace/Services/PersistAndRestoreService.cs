//https://github.com/microsoft/TemplateStudio?tab=readme-ov-file
//Template Studio
//Copyright (c) .NET Foundation and Contributors.

//All rights reserved.

//MIT License
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


//using DataLoggerDotNet.Domain.StaticValues;
//using DataLoggerDotNet.Infrastructure.Json;
//using DataLoggerDotNet.WPF.Interfaces;
//using Microsoft.Extensions.Options;
//using System.Collections;

//namespace Trace.Services
//{
//    class PersistAndRestoreService : IPersistAndRestoreService
//    {
//        const string FILE_NAME = "ApplicationSettings" + FileExtensions.Json;
//        private readonly string _folderPath = AppConfig.AppDataFolder;

//        public PersistAndRestoreService()
//        {
//        }

//        public void PersistData()
//        {
//            if (App.Current.Properties != null)
//            {
//                JsonHelper.Save(_folderPath, FILE_NAME, App.Current.Properties);
//            }
//        }

//        public void RestoreData()
//        {
//            var properties = JsonHelper.Read<IDictionary>(_folderPath, FILE_NAME);
//            if (properties != null)
//            {
//                foreach (DictionaryEntry property in properties)
//                {
//                    App.Current.Properties.Add(property.Key, property.Value);
//                }
//            }
//        }
//    }
//}
