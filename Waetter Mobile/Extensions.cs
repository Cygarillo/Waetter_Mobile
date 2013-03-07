﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace Waetter_Mobile
{
    public static class Extensions
    {
        public static Task<string> DownloadStringTask(this WebClient webClient, Uri uri)
        {
            var tcs = new TaskCompletionSource<string>();

            webClient.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            webClient.DownloadStringAsync(uri);

            return tcs.Task;
        }
    }
}
