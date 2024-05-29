﻿using STranslate.Log;
using STranslate.Util;
using System;
using System.Windows.Forms;

namespace STranslate.Helper
{
    public class MouseHookHelper
    {
        private readonly MouseHookUtil mouseHook;

        private bool isDown = false;

        private bool isMove = false;

        private bool isStart = false;

        public Action<string>? OnGetwordsHandler;

        public MouseHookHelper()
        {
            mouseHook = new MouseHookUtil();
        }

        public void MouseHookStart()
        {
            mouseHook.MouseMove += mouseHook_MouseMove;
            mouseHook.MouseDown += mouseHook_MouseDown;
            mouseHook.MouseUp += mouseHook_MouseUp;
            mouseHook.Start();
            isStart = true;
        }

        public void MouseHookStop()
        {
            mouseHook.MouseMove -= mouseHook_MouseMove;
            mouseHook.MouseDown -= mouseHook_MouseDown;
            mouseHook.MouseUp -= mouseHook_MouseUp;
            mouseHook.Stop();
            isStart = false;
        }

        private void mouseHook_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDown = true;
            }
        }

        private void mouseHook_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDown && isStart)
            {
                isMove = true;
            }
        }

        private void mouseHook_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (isDown && isMove)
            {
                var interval = Singleton<ConfigHelper>.Instance.CurrentConfig?.WordPickingInterval ?? 100;
                var content = ClipboardUtil.GetSelectedTextDiff(interval);
                if (string.IsNullOrEmpty(content))
                {
                    LogService.Logger.Debug($"可能是取词内容相同,或者需要增加取词延迟(当前: {interval}ms)...");
                    return;
                }
                OnGetwordsHandler?.Invoke(content);
            }
            isDown = false;
            isMove = false;
        }
    }
}