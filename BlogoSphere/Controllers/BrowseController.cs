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
            var blogs = db.Blogs.Include(b => b.Author).ToList();

            //if (blogs != null)
            //    return View();

			foreach (var blog in blogs)
			{
                int totalViews = 0;
				foreach (var post in blog.Posts)
				{
                    totalViews += post.Views;
				}
                blog.Views = totalViews;
			}

            return View(blogs.OrderByDescending(b => b.Views).ToList());
        }

        public ActionResult BrowseTagTabs(int? tagId)
        {
            ViewBag.BrowseTagId = tagId;

            var popularTags = db.Posts
                .SelectMany(p => p.Tags)
                .GroupBy(t => t, (k, g) => new { Tag = k, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Select(g => g.Tag)
                .ToList();

            return View(popularTags.Take(20).ToList());           
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