using PHANMEM_SIMPLENOTE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHANMEM_SIMPLENOTE.Controllers
{
    public class TagControllers
    {

        public static List<Tag> getAllTag()
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from gettag in _context.Tags.AsEnumerable()
                           select new
                           {
                               TenTag = gettag.TenTag,
                               Notes = gettag.Notes
                           })
                            .Select(x => new Tag
                            {
                                TenTag = x.TenTag,
                                Notes = x.Notes
                            }).ToList();
                return get;
            }
        }


        public static bool addTag(Tag tag)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return true;
            }
        }

        public static bool deleteTag(Tag tag)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from gettag in _context.Tags
                             where gettag.TenTag == tag.TenTag
                             select gettag).SingleOrDefault();


                foreach (var note in get.Notes)
                {
                    foreach (var temp in note.Tags)
                    {
                        if (temp.TenTag == tag.TenTag)
                        {
                            note.Tags.Remove(temp);
                            break;
                        }
                    }
                }
                _context.Tags.Remove(get);
                _context.SaveChanges();
                return true;
            }
        }

        public static Tag getoneTag(string tentag)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from gettag in _context.Tags
                           where gettag.TenTag == tentag
                           select gettag).ToList();

                if (get.Count == 1)
                    return get[0];
                else
                    return null;
            }
        }

        public static List<Tag> getAllTagbyString(string tentag)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from gettag in _context.Tags.AsEnumerable()
                           where gettag.TenTag.Contains(tentag)
                           select new
                           {
                               TenTag = gettag.TenTag,
                               Notes = gettag.Notes
                           })
                            .Select(x => new Tag
                            {
                                TenTag = x.TenTag,
                                Notes = x.Notes
                            }).ToList();
                return get;
            }
        }
    }
}
