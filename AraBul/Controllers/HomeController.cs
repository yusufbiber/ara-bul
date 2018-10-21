using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AraBul.Models;
using System.IO;

namespace AraBul.Controllers
{
    public class HomeController : Controller
    {        
        IndexEngine.Engine engine;
        public HomeController(IndexEngine.Engine engine)
        {
            this.engine = engine;
        }
        public IActionResult Index(string txt)
        {
            SearchResult searchResult = new SearchResult();
            if (txt!=null) { 
                this.engine.Search(txt); 
                searchResult.files = this.engine.res;
            }

            return View(searchResult);
        }

        public IActionResult About()
        {

            //System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(@"D:\ProjectsDDI");

           // long size = GetDirectorySize(@"D:\ProjectsDDI\BPM\AdvancedBPMClient");

            this.engine.Start(@"D:\ProjectsDDI\BPM\AdvancedBPMClient");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        private static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
