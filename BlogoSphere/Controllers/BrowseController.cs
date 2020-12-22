﻿using BlogoSphere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BlogoSphere.Controllers
{
    public class BrowseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Browse
        public ActionResult BrowseViews()
        {
            var blogs = db.Blogs.Include(b => b.Author).OrderByDescending(v => v.Views).ToList();
            if (blogs != null)
                return View(blogs);
            return View(blogs);
        }

        public ActionResult BrowseTagTabs(int? tagId)
        {
            ViewBag.BrowseTagId = tagId;

            var PoularTags = db.Tags.ToList();
            return View(PoularTags);           
        }
        
        public ActionResult BrowseTags(int? tagId)
        {
            List<Post> tagResult = null;
            if (tagId != null && db.Tags.Any(t => t.Id == tagId))
                tagResult = db.Posts
                    .Include(p => p.Blog)
                    .Include(p=>p.Blog.Author)
                    .Where(p => p.Tags.Any(t => t.Id == tagId)).ToList();

            return View(tagResult);
        }
    }
}