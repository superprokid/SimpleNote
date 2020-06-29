using PHANMEM_SIMPLENOTE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHANMEM_SIMPLENOTE.Controllers
{
    public class NoteControllers
    {
        public static bool addNote(Note note)
        {
            try
            {
                using (var _context = new DbofSimpleNoteEntities())
                {
                    foreach (var temp in note.Tags)
                    {
                        var get = (from gettag in _context.Tags
                                   where gettag.TenTag == temp.TenTag
                                   select gettag).Single();
                        get.Notes.Add(note);
                    }
                    note.Tags.Clear();
                    _context.Notes.Add(note);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool updateNote(Note note)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                _context.Notes.AddOrUpdate(note);
                _context.SaveChanges();
                return true;
            }
        }

        public static bool deleteNote(Note note)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from getnote in _context.Notes
                              where getnote.SoThuTu == note.SoThuTu
                              select getnote).SingleOrDefault();
                foreach (var gettag in get.Tags)
                {
                    foreach (var temp in gettag.Notes)
                    {
                        if (temp.SoThuTu == note.SoThuTu)
                        {
                            gettag.Notes.Remove(temp);
                            break;
                        }
                    }
                }
                _context.Notes.Remove(get);
                _context.SaveChanges();
                return true;
            }
        }

        public static int getIdFromDb()
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var stt = (from getnote in _context.Notes
                           select getnote.SoThuTu).ToList();

                if(stt.Count <= 0)
                {
                    return 1;
                }
                return stt.Max() + 1;
            }
        }

        public static Note getNote(int stt)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from getnote in (from note in _context.Notes.AsEnumerable()
                                       select note)
                            where getnote.SoThuTu == stt
                            select new
                            {
                                SoThuTu = getnote.SoThuTu,
                                TieuDe = getnote.TieuDe,
                                NoiDung = getnote.NoiDung,
                                ThongTin = getnote.ThongTin,
                                Rac = getnote.Rac,
                                PintoTop = getnote.PintoTop,
                                Tags = getnote.Tags
                            })
                            .Select(x => new Note
                            {
                                SoThuTu = x.SoThuTu,
                                TieuDe = x.TieuDe,
                                NoiDung = x.NoiDung,
                                ThongTin = x.ThongTin,
                                Rac = x.Rac,
                                PintoTop = x.PintoTop,
                                Tags = x.Tags
                            }).SingleOrDefault();
                return get;
            }
        }

        public static List<Note> getAllNotes()
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from getnote in _context.Notes.AsEnumerable()
                             select new
                             {
                                 SoThuTu = getnote.SoThuTu,
                                 TieuDe = getnote.TieuDe,
                                 NoiDung = getnote.NoiDung,
                                 ThongTin = getnote.ThongTin,
                                 Rac = getnote.Rac,
                                 PintoTop = getnote.PintoTop,
                                 Tags = getnote.Tags
                             })
                            .Select(x => new Note
                            {
                                SoThuTu = x.SoThuTu,
                                TieuDe = x.TieuDe,
                                NoiDung = x.NoiDung,
                                ThongTin = x.ThongTin,
                                Rac = x.Rac,
                                PintoTop = x.PintoTop,
                                Tags = x.Tags
                            }).ToList();
                return get;
            }
        }

        public static List<Note> getAllNotesbyString(string find)
        {
            using (var _context = new DbofSimpleNoteEntities())
            {
                var get = (from note in _context.Notes.AsEnumerable()
                             where note.NoiDung.Contains(find)
                             select new
                             {
                                 SoThuTu = note.SoThuTu,
                                 TieuDe = note.TieuDe,
                                 NoiDung = note.NoiDung,
                                 ThongTin = note.ThongTin,
                                 Rac = note.Rac,
                                 PintoTop = note.PintoTop,
                                 Tags = note.Tags
                             })
                            .Select(x => new Note
                            {
                                SoThuTu = x.SoThuTu,
                                TieuDe = x.TieuDe,
                                NoiDung = x.NoiDung,
                                ThongTin = x.ThongTin,
                                Rac = x.Rac,
                                PintoTop = x.PintoTop,
                                Tags = x.Tags
                            }).ToList();
                return get;
            }
        }
    }
}
