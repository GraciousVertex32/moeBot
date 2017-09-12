using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using moeInterface;

namespace Plugins
{
    class Plugin
    {
        // Let info.json Load in
        // Parse the info.json


        /********************************************************************
         * {
         *   "name": "Example Plugin",
         *   "id": "expl",
         *   "alternatedid": "explP",
         *   "version": "1.0.0",
         *   "compatible-version": "(1.0.0",
         *   "description": "This is A Example Plugin Created by Your Cat.",
         *   "repository":
         *   {
         *     "type": "git",
         *     "url": "https://github.com/example/moeBot-plugin-example"
         *   },
         * }
         ********************************************************************/


        // name
        // id
        // 
        public static void LoadIn()
        {
            Loader l = new Loader();
            l.Plugin;

        }

        
    }

    class Loader
    {
        public void Plugin()
        {
            string PluginName = null;
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory; 

            if (Load().Count == 0)
            {
                Console.WriteLine("No Plugins Found.");
                return;
            }

            Dictionary<string, IPlugin> _Plugins = new Dictionary<string, IPlugin>();
            ICollection<IPlugin> plugins = Load();
            Console.WriteLine("All plugins loaded.");

            foreach (var plugin in plugins)
            {
                _Plugins.Add(plugin.Name, plugin);
                PluginName = plugin.Name;
                Console.WriteLine(PluginName);

                try
                {
                    Console.WriteLine(_Plugins[PluginName].Hello());
                    _Plugins[PluginName].Start();
                    _Plugins[PluginName].applicationPath(applicationPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private ICollection<IPlugin> Load()
        {
            //Load the plugin into Assembly and return the ICollection back

            Console.WriteLine("Initializing Plugins...");
            List<string> dllFileNames = PathFinder();        //载入文件名
            ICollection<Assembly> assemblies = new List<Assembly>(PathFinder().Count);  
            //容量为dll文件数量，变量类型为assembly（？），的新collection

            foreach (string dllFile in dllFileNames)    
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);    //为assemblyname（string？）属性赋值并命名为an
                Console.WriteLine(an);//显示插件名
                Assembly assembly = Assembly.Load(an);//直接载入 ？
                assemblies.Add(assembly);//为collection加入新的assembly
            }

            Type pluginType = typeof(IPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            ICollection<IPlugin> plugins = new List<IPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }
            return plugins;
        }
        
        //Find Path
        
        private List<string> PathFinder()    //UNDERSTOOD
        {
            List<string> pluginpath = new List<string>();
            try
            {
                //获取程序的基目录
                string path = AppDomain.CurrentDomain.BaseDirectory;      //得到根目录
                //合并路径，指向插件所在目录。
                path = Path.Combine(path, "Plugins");                    //根目录下Plugins文件夹
                foreach (string filename in Directory.GetFiles(path, "*.dll"))    //插件所在文件夹下dll文件名
                {
                    pluginpath.Add(filename);             //文件名加入list
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return pluginpath;                      //把pluginpath返回patherfinder方式的结果
        }
    }

    class Initializer
    {
        public static void PreInitialization()
        {
            //Todo
        }
        public static void Initialization()
        {
            //Todo

        }
    }
}
 