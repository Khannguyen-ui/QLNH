// GUI_QLNH/Common/ExportPolicy.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GUI_QLNH.Common
{
    /// <summary>
    /// Centralized export permission policy. Reads from export_policy.json or export_policy.sample.json if present.
    /// If no policy file exists, allow exports for common forms by default.
    /// </summary>
    public static class ExportPolicy
    {
        private static readonly string PolicyFileName = "export_policy.json";
        private static readonly string SampleFileName = "export_policy.sample.json";
        private static DateTime _lastRead;
        private static PolicyModel _cache = new PolicyModel
        {
            enabled = true,
            forms = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        };

        public class PolicyModel
        {
            public bool enabled { get; set; }
            public Dictionary<string, bool> forms { get; set; }
        }

        private static void EnsureDefaultsIfNoFile()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, PolicyFileName);
            var samplePath = Path.Combine(baseDir, SampleFileName);
            if (File.Exists(path) || File.Exists(samplePath)) return;

            if (_cache.forms == null || _cache.forms.Count == 0)
            {
                _cache.forms = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
 {
 { "NhanVien", true },
 { "ThucKhach", true },
 { "ThucDon", true },
 { "HoaDon", true },
 { "DatTiec", true },
 { "ThongKe", true }
 };
            }
        }

        public static bool IsExportAllowed(string key)
        {
            try
            {
                Refresh();
                EnsureDefaultsIfNoFile();

                if (!_cache.enabled) return false;
                if (_cache.forms == null || _cache.forms.Count == 0) return true;
                if (string.IsNullOrWhiteSpace(key)) return false;
                if (_cache.forms.TryGetValue(key, out var allowed)) return allowed;
                // default allow
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void Refresh()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, PolicyFileName);
            var samplePath = Path.Combine(baseDir, SampleFileName);

            string loadPath = null;
            if (File.Exists(path)) loadPath = path;
            else if (File.Exists(samplePath)) loadPath = samplePath;

            if (loadPath == null) return;

            var info = new FileInfo(loadPath);
            if (_lastRead != default && info.LastWriteTime <= _lastRead) return;

            var json = File.ReadAllText(loadPath);
            try
            {
                var policy = JsonConvert.DeserializeObject<PolicyModel>(json);
                if (policy != null)
                {
                    if (policy.forms == null) policy.forms = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    else
                    {
                        var normalized = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                        foreach (var kv in policy.forms) normalized[kv.Key ?? string.Empty] = kv.Value;
                        policy.forms = normalized;
                    }

                    _cache = policy;
                    _lastRead = info.LastWriteTime;
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}
