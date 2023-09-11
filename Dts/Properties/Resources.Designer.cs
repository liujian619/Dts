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
        ///   查找类似 配置文件（UTF-8 格式）说明：
        ///
        ///    1. 以“#”开头：注释；
        ///
        ///    2. 以“+wf:”开头：指定用以匹配文件的通配符，通过该通配符匹配到的文件集合会被添加到解析结果中；
        ///
        ///    3. 以“-wf:”开头：指定用以匹配文件的通配符，通过该通配符匹配到的文件集合会从解析结果中移除；
        ///
        ///    4. 以“+rf:”开头：指定用以匹配文件的正则表达式，通过该正则表达式匹配到的文件集合会被添加到解析结果中；
        ///
        ///    5. 以“-rf:”开头：指定用以匹配文件的正则表达式，通过该正则表达式匹配到的文件集合会从解析结果中移除；
        ///
        ///    6. 以“+wd:”开头：指定用以匹配目录的通配符，通过该通配符匹配到的目录集合会被添加到解析结果中；
        ///
        ///    7. 以“-wd:”开头：指定用以匹配目录的通配符，通过该通配符匹配到的目录集合会从解析结果中移除；
        ///
        ///    8. 以“+rd:”开头：指定用以匹配目录的正则表达式，通过该正则表达式匹配到的目录集合会被添加到解析结果中；
        ///
        ///    9. 以“-rd:”开头：指定用以匹配目录的正则表达式，通过该正则表达式匹配到的目录集合 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string detail {
            get {
                return ResourceManager.GetString("detail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 名称：
        ///    
        ///    Dts
        ///
        ///说明：
        ///    
        ///    用于从文件中提取特定格式的 JsDoc 文档注释以生成 .d.ts 文件。
        ///
        ///使用说明：
        ///    
        ///    dts -f &lt;file&gt;... [-m &lt;output file&gt;]
        ///
        ///    dts -d &lt;dir&gt; [&lt;dtssetting file&gt;]
        ///
        ///    dts -h | --help
        ///    
        ///    dts -H | --extended-help
        ///
        ///    dts -v
        ///
        ///选项说明：
        ///
        ///    -f &lt;file&gt;...            1 个或多个输入文件
        ///    
        ///    -d &lt;dir&gt;                输入文件所在的目录
        ///       &lt;dtssetting file&gt;    设置文件，相对于 &lt;dir&gt; 目录，默认为“.dtssetting”
        ///
        ///    -m &lt;output file&gt;        开启合并，合并所有输出到 &lt;output file&gt;
        ///
        ///    -h, --help              查看帮助
        ///
        ///    -H, --e [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string help {
            get {
                return ResourceManager.GetString("help", resourceCulture);
            }
        }
    }
}
