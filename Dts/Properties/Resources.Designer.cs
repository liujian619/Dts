﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dts.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Dts.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 【命令参数说明】
        ///
        ///-h, -H			查看帮助
        ///path[, config]		用于搜索的路径；config 为配置文件路径，默认为 path 目录下 .dtssetting 文件
        ///
        ///
        ///【配置文件说明】
        ///
        ///配置文件必须是 UTF-8 格式
        ///
        ///1. 以“#”开头：注释；
        ///
        ///2. 以“+wf:”开头：指定用以匹配文件的通配符，通过该通配符匹配到的文件集合会被添加到解析结果中；
        ///
        ///3. 以“-wf:”开头：指定用以匹配文件的通配符，通过该通配符匹配到的文件集合会从解析结果中移除；
        ///
        ///4. 以“+rf:”开头：指定用以匹配文件的正则表达式，通过该正则表达式匹配到的文件集合会被添加到解析结果中；
        ///
        ///5. 以“-rf:”开头：指定用以匹配文件的正则表达式，通过该正则表达式匹配到的文件集合会从解析结果中移除；
        /// 
        ///6. 以“+wd:”开头：指定用以匹配目录的通配符，通过该通配符匹配到的目录集合会被添加到解析结果中；
        /// 
        ///7. 以“-wd:”开头：指定用以匹配目录的通配符，通过该通配符匹配到的目录集合会从解析结果中移除；
        /// 
        ///8. 以“+rd:”开头：指定用以匹配目录的正则表达 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string help {
            get {
                return ResourceManager.GetString("help", resourceCulture);
            }
        }
    }
}
