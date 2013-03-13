/***************************************************************************
 *   Copyright (C) 2012 by Jerry Liang                                     *
 *   jerry27syd@gmail.com                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace NoSqlJsonFileProject
{
    [DataContract]
    public class NoSqlJsonFile<T> where T : new()
    {
        //THIS MUST BE SET MANUALLY
        public static string FILE_PATH = @"c:\temp";
        //Set it to false will increase speed but large storage, set it to true will slow down speed but with optimized storage
        private static readonly DirectoryInfo _defaultDirectory = new DirectoryInfo(Path.Combine(FILE_PATH, typeof(T).Name));

        public NoSqlJsonFile()
        {
            DateModfied = DateTime.Now;
            UniqueId = GetType().Name + Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        static NoSqlJsonFile()
        {
            SaveOptimizationEnable = false;
        }

        /// <summary>
        ///  Internal Date Stamp.
        /// </summary>
        [DataMember]
        public DateTime DateModfied { get; set; }

        /// <summary>
        /// Experiment Feature.
        /// </summary>
        public static bool SaveOptimizationEnable { get; set; }

        /// <summary>
        ///  UniqueId is a unique number for each file created.
        /// </summary>
        [DataMember]
        public string UniqueId { get; set; }

        public static DirectoryInfo DefaultDirectory
        {
            get { return _defaultDirectory; }
        }

        public FileInfo GetFileId()
        {
            return new FileInfo(Path.Combine(DefaultDirectory.FullName, UniqueId));
        }

        public static FileInfo GetFileId(string uniqueId)
        {
            return new FileInfo(Path.Combine(DefaultDirectory.FullName, uniqueId));
        }

        /// <summary>
        /// Delete current file only. 
        /// </summary>
        public void Delete()
        {
            GetFileId().Delete();
        }

        public void DeleteCascade()
        {
            DeleteRecursive(this);
        }

        /// <summary>
        /// Static method of DeleteCascade.
        /// </summary>
        /// <param name="uniqueId"></param>
        public static void DeleteCascade(string uniqueId)
        {
            var t = new T();
            t.GetType().GetProperty("UniqueId").SetValue(t, uniqueId, null);
            GetBreathFirst(t);
            t.GetType()
             .GetMethod("DeleteCascade", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public)
             .Invoke(t, null);
        }

        public void DeleteRecursive(object obj)
        {
            if (ReflectionBaseTypeCompare(obj.GetType(), typeof(NoSqlJsonFile<>)))
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(NoSqlJsonFile<>)))
                    {
                        object item = propertyInfo.GetValue(obj, null);
                        if (item != null)
                        {
                            DeleteRecursive(item);
                        }
                    }
                    else if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(List<>)) ||
                             ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(ObservableCollection<>)))
                    {
                        var list = (IList) propertyInfo.GetValue(obj, null);
                        if (list != null)
                        {
                            foreach (object item in list)
                            {
                                DeleteRecursive(item);
                            }
                        }
                    }
                }
                obj.GetType()
                   .GetMethod("Delete", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public)
                   .Invoke(obj, null);
            }
        }

        public bool Exists()
        {
            FileInfo fileId = GetFileId();
            return fileId.Exists;
        }
        /// <summary>
        /// Static method of Exists
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public static bool Exists(string uniqueId)
        {
            var fileId = new FileInfo(Path.Combine(DefaultDirectory.FullName, uniqueId));
            return fileId.Exists;
        }

        public void Get()
        {
            try
            {
                GetBreathFirst(this);
            }
            catch (IOException)
            {
                throw new Exception("File Not Found.");
            }
        }

        public void Save(bool checkModified = false)
        {
            DateModfied = DateTime.Now;
            SaveRecursive(this, checkModified);
            if (SaveOptimizationEnable) Get();
        }

        public void SaveIfModified()
        {
            SaveRecursive(this, true);
            if (SaveOptimizationEnable) Get();
        }

        public static void Delete(string uniqueId)
        {
            FileInfo fileId = GetFileId(uniqueId);
            fileId.Delete();
        }

        public static T Get(string uniqueId)
        {
            var t = new T();
            t.GetType().GetProperty("UniqueId").SetValue(t, uniqueId, null);
            GetBreathFirst(t);
            return t;
        }

        public static void Save(NoSqlJsonFile<T> t, bool checkModified = false)
        {
            SaveRecursive(t, checkModified);
        }

        public static void SaveIfModified(NoSqlJsonFile<T> t)
        {
            SaveRecursive(t, true);
        }

        public static void CleanDirectory()
        {
            DirectoryInfo dir = DefaultDirectory;
            foreach (FileInfo fileId in dir.GetFiles())
            {
                fileId.Delete();
            }
        }

        #region Listing Implmentation
        public static List<T> List()
        {
            var resultList = new List<T>();
            try
            {
                DirectoryInfo dir = DefaultDirectory;
                foreach (FileInfo fileId in dir.GetFiles())
                {
                    var t = new T();
                    if (fileId.Name.Contains(t.GetType().Name))
                    {
                        t.GetType().GetProperty("UniqueId").SetValue(t, fileId.Name, null);
                        t.GetType().GetMethod("Get", new Type[] { }).Invoke(t, null);
                        resultList.Add(t);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }
            return resultList;
        }

        public static List<TList> List<TList>()
        {
            var resultList = new List<TList>();
            try
            {
                DirectoryInfo dir = DefaultDirectory;
                foreach (FileInfo fileId in dir.GetFiles())
                {
                    object t = Activator.CreateInstance(typeof(TList));
                    if (fileId.Name.Contains(t.GetType().Name))
                    {
                        t.GetType().GetProperty("UniqueId").SetValue(t, fileId.Name, null);
                        t.GetType().GetMethod("Get", new Type[] { }).Invoke(t, null);
                        resultList.Add((TList) t);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
            return resultList;
        }

        public static IEnumerable<T> Enumerable()
        {
            if (!DefaultDirectory.Exists) yield break;

            FileInfo[] files = DefaultDirectory.GetFiles();
            if (files.Length < 0)
            {
                yield break;
            }
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileId = files[i];
                var t = new T();
                if (fileId.Name.Contains(t.GetType().Name))
                {
                    t.GetType().GetProperty("UniqueId").SetValue(t, fileId.Name, null);
                    t.GetType().GetMethod("Get", new Type[] { }).Invoke(t, null);
                    yield return t;
                }
            }
        }

        public static IEnumerable<TEnumerable> Enumerable<TEnumerable>()
        {
            if (!DefaultDirectory.Exists) yield break;

            FileInfo[] files = DefaultDirectory.GetFiles();
            if (files.Length < 0)
            {
                yield break;
            }
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileId = files[i];
                object t = Activator.CreateInstance(typeof(TEnumerable));
                if (fileId.Name.Contains(t.GetType().Name))
                {
                    t.GetType().GetProperty("UniqueId").SetValue(t, fileId.Name, null);
                    t.GetType().GetMethod("Get", new Type[] { }).Invoke(t, null);
                    yield return (TEnumerable) t;
                }
            }
        }

        public static ObservableCollection<T> ObservableCollection()
        {
            var resultList = new ObservableCollection<T>();
            try
            {
                DirectoryInfo dir = DefaultDirectory;
                foreach (FileInfo fileId in dir.GetFiles())
                {
                    var t = new T();
                    if (fileId.Name.Contains(t.GetType().Name))
                    {
                        t.GetType().GetProperty("UniqueId").SetValue(t, fileId.Name, null);
                        t.GetType().GetMethod("Get", new Type[] { }).Invoke(t, null);
                        resultList.Add(t);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
            return resultList;
        }

        public static List<string> ListUniqueIds()
        {
            var resultList = new List<string>();
            try
            {
                DirectoryInfo dir = DefaultDirectory;
                foreach (FileInfo fileId in dir.GetFiles())
                {
                    resultList.Add(fileId.Name);
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
            return resultList;
        }
        #endregion

        #region Read Implmenation
        protected static void GetBreathFirst(object obj)
        {
            object val =
                obj.GetType()
                   .GetMethod("Deserialize", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic)
                   .Invoke(obj, null);
            CopyObject(val, ref obj);
            var rootQueue = new Queue<object>();
            EnqueueChildren(obj, rootQueue);
            while (rootQueue.Count > 0)
            {
                object next = rootQueue.Dequeue();

                if (ReflectionBaseTypeCompare(next.GetType(), typeof(NoSqlJsonFile<>)))
                {
                    object newObj =
                   next.GetType()
                       .GetMethod("Deserialize",
                                  BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic)
                       .Invoke(next, null);
                    CopyObject(newObj, ref next);

                }

                EnqueueChildren(next, rootQueue);
            }
        }

        protected static void EnqueueChildren(object obj, Queue<object> rootQueue)
        {
            if (ReflectionBaseTypeCompare(obj.GetType(), typeof(NoSqlJsonFile<>)))
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(NoSqlJsonFile<>)))
                    {
                        object item = propertyInfo.GetValue(obj, null);
                        if (item != null)
                        {
                            rootQueue.Enqueue(item);
                        }
                    }
                    else if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(List<>)) ||
                             ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(ObservableCollection<>)))
                    {
                        var list = (IList) propertyInfo.GetValue(obj, null);
                        if (list != null)
                        {
                            foreach (object item in list)
                            {
                                rootQueue.Enqueue(item);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Recursive implmentation of Save function.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="checkModified"></param>
        protected static void SaveRecursive(object obj, bool checkModified)
        {
            //if base type is DataEntity, then we treat it as entity
            if (ReflectionBaseTypeCompare(obj.GetType(), typeof(NoSqlJsonFile<>)))
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(NoSqlJsonFile<>)))
                    {
                        object item = propertyInfo.GetValue(obj, null);
                        if (item != null)
                        {
                            SaveRecursive(item, checkModified);
                            // Clean Entity content at current depth  
                            if (SaveOptimizationEnable) CleanEntityContent(item);
                        }
                    }
                    else if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(List<>)) ||
                             ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(ObservableCollection<>)))
                    {
                        var list = (IList) propertyInfo.GetValue(obj, null);
                        if (list != null)
                        {
                            foreach (object item in list)
                            {
                                SaveRecursive(item, checkModified);
                                // Clean Entity content at current depth  
                                if (SaveOptimizationEnable) CleanEntityContent(item);
                            }
                        }
                    }
                }

                if (checkModified)
                {
                    var modified = (bool) obj.GetType().GetProperty("Modified").GetValue(obj, null);
                    if (modified)
                        obj.GetType()
                           .GetMethod("Serialize",
                                      BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic)
                           .Invoke(obj, null);
                }
                else
                {
                    obj.GetType()
                       .GetMethod("Serialize",
                                  BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic)
                       .Invoke(obj, null);
                }
            }
        }

        protected object Deserialize()
        {
            FileInfo fileId = GetFileId();
            return DeserializeFromFile(this, fileId);
        }

        protected void Serialize()
        {
            FileInfo fileId = GetFileId();
            SerializeToFile(this, fileId);
        }

        protected static void SerializeToFile(object obj, FileInfo fileId)
        {
            CreateDirectoryIfNotExists(fileId.Directory);
            File.WriteAllText(fileId.FullName, ToJson(obj));
        }

        protected static object DeserializeFromFile(object obj, FileInfo fileId)
        {
            object newObj = JsonToObject(File.ReadAllText(fileId.FullName), obj.GetType());
            return newObj;
        }

        private static void CleanEntityContent(object obj)
        {
            CleanEntityContentRecursive(obj);
        }

        private static void CleanEntityContentRecursive(object obj)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(NoSqlJsonFile<>)))
                {
                    object item = propertyInfo.GetValue(obj, null);
                    if (item != null)
                    {
                        CleanEntityContentRecursive(item);
                    }
                }
                else if (ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(List<>)) ||
                         ReflectionBaseTypeCompare(propertyInfo.PropertyType, typeof(ObservableCollection<>)))
                {
                    IList list = (IList) propertyInfo.GetValue(obj, null);
                    if (list != null)
                    {
                        foreach (object item in list)
                        {
                            CleanEntityContentRecursive(item);
                        }
                    }
                }
                else
                {
                    if (propertyInfo.Name != "UniqueId" && propertyInfo.DeclaringType != typeof(Guid))
                    {
                        propertyInfo.SetValue(obj, GetDefault(propertyInfo.PropertyType), null);
                    }
                }
            }
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private static bool ReflectionBaseTypeCompare(Type t, Type t2)
        {
            if (t.FullName.StartsWith(t2.FullName)) return true;
            if (t == typeof(object)) return false;
            return ReflectionBaseTypeCompare(t.BaseType, t2);
        }

        private static void CreateDirectoryIfNotExists(DirectoryInfo dirInfo)
        {
            if (dirInfo == null) return;
            if (dirInfo.Parent != null) CreateDirectoryIfNotExists(dirInfo.Parent);
            if (!dirInfo.Exists) dirInfo.Create();
        }

        private static void CopyObject(object sourceObject, ref object destObject)
        {
            //	If either the source, or destination is null, return
            if (sourceObject == null || destObject == null)
                return;

            //	Get the type of each object
            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            //	Loop through the source properties
            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                //	Get the matching property in the destination object
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                //	If there is none, skip
                if (targetObj == null)
                    continue;

                //	Set the value in the destination
                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }

        public static string ToJson(object value)
        {
            if (value == null)
            {
                return null;
            }

            var serializer = new DataContractJsonSerializer(value.GetType());
            using (var dataInMemory = new MemoryStream())
            {
                serializer.WriteObject(dataInMemory, value);
                return Encoding.Default.GetString(dataInMemory.ToArray());
            }
        }

        public static object JsonToObject(string xml, Type t)
        {
            if (xml == null || t == null)
            {
                return null;
            }

            using (var dataInMemory = new MemoryStream(Encoding.Default.GetBytes(xml)))
            {
                return new DataContractJsonSerializer(t).ReadObject(dataInMemory);
            }
        }
    }

}