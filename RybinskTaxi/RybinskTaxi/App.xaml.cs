﻿using RybinskTaxi.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону "Приложение таблицы" см. по адресу http://go.microsoft.com/fwlink/?LinkId=234226

namespace RybinskTaxi
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая строка разрабатываемого кода
        /// кода; поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
        /// если приложение запускается для открытия конкретного файла, отображения
        /// результатов поиска и т. д.
        /// </summary>
        /// <param name="args">Сведения о запросе и обработке запуска.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();
                //Связывание фрейма с ключом SuspensionManager                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Восстановление сохраненного состояния сеанса только при необходимости
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Возникли ошибки при восстановлении состояния.
                        //Предполагаем, что состояние отсутствует, и продолжаем
                    }
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // Если стек навигации не восстанавливается для перехода к первой странице,
                // настройка новой страницы путем передачи необходимой информации в качестве параметра
                // навигации
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения. Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Вызывается при активации приложения для отображения результатов поиска.
        /// </summary>
        /// <param name="args">Сведения о запросе на активацию.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Регистрация события Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // в OnWindowCreated для ускорения поиска во время выполнения приложения

            // Если в окне еще не используется навигация по фреймам, вставьте собственный фрейм
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // Если приложение не содержит фрейм верхнего уровня, то, возможно, это
            // первый запуск приложения. Обычно этот метод и метод OnLaunched 
            // из файла App.xaml.cs могут вызывать общий метод.
            if (frame == null)
            {
                // Создание фрейма, играющего роль контекста навигации, и его связывание с
                // ключом SuspensionManager
                frame = new Frame();
                RybinskTaxi.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Восстановление сохраненного состояния сеанса только при необходимости
                    try
                    {
                        await RybinskTaxi.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (RybinskTaxi.Common.SuspensionManagerException)
                    {
                        //Возникли ошибки при восстановлении состояния.
                        //Предполагаем, что состояние отсутствует, и продолжаем
                    }
                }
            }

            frame.Navigate(typeof(TaxiSearch), args.QueryText);
            Window.Current.Content = frame;

            // Обеспечение активности текущего окна
            Window.Current.Activate();
        }
    }
}
