using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mono.Cecil;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AssemblyLoad(String Path)
        {
            //Для преобразования строки символы /r/n
            String text;
           
            //формирование строки для удобства наполнения
            StringBuilder str_bldr = new StringBuilder();


      

                //Директория
                System.IO.DirectoryInfo diDir = new DirectoryInfo(Path);
                //Сами файлы длл
                String[] File_Array1 = Directory.GetFiles(Path, "*.dll"); ;
                 //Цикл по всем файлам
                foreach (String lfileInfo in File_Array1)
                {
                    //Иннициализвация monocecil
                    AssemblyDefinition asm = AssemblyDefinition.ReadAssembly(lfileInfo);
                     //Тип модуля класс программы  м т.д.
                    foreach (var type in asm.MainModule.Types)
                    {
                         //Лишние убрать
                        if (type.Name != "<Module>")
                        {
                            //Добваление класса
                            str_bldr.AppendLine(type.Name);
                        }
                        // Добавление методов 
                        foreach (var method in type.Methods)
                        {
                          //protectec и public
                        if (method.GetType().IsNotPublic || method.GetType().IsPublic)
                            {
                                 // Убрать лишние
                                if (method.Name != ".ctor")
                                {
                                     //Добавление методов к классу конретному впемереди класс
                                    str_bldr.AppendLine("--"+method.Name);
                                }

                            }
                        }


                    }
      
                }
                             
 

            //преобразование стрки симоволов в строку c \r\n
            text = str_bldr.ToString();
            //создание файла 
            using (StreamWriter swriter = new StreamWriter(Path + "\\method.txt"))
            {
                //Завпись файла
                swriter.Write(text);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Выбор директории
            String Dir_Base="";
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Выбор диерктории для поиска метов в длл";
                
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                   Dir_Base = dlg.SelectedPath;
                }
            }
            //Поиск файлов в директории
            
             AssemblyLoad(Dir_Base);
            
            
                        //Вызов функции для поиска методов и классов 
                      
        }
    }
}
