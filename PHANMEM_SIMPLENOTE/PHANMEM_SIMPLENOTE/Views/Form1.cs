using PHANMEM_SIMPLENOTE.Controllers;
using PHANMEM_SIMPLENOTE.Models;
using PHANMEM_SIMPLENOTE.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHANMEM_SIMPLENOTE
{
    public partial class Form1 : Form
    {
       
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            rtbTypeNoiDung.SelectionFont = fontDialog1.Font;

        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            rtbTypeNoiDung.SelectionColor = colorDialog1.Color;
        }

        public Form1()
        {
            InitializeComponent();
            this.panel1.SendToBack();
            this.rtbShowTag.ReadOnly = true;
            StateofMenu = true;
            this.cNoiDung.Text = "Nội Dung";
            this.btnNewNote.Visible = true;
            this.btnDelete.Visible = true;
            this.btnInfo.Visible = true;
            this.toolStrip3.Visible = true;
            this.lbInfo.Text = "";
            this.rtbTypeNoiDung.ReadOnly = false;
            this.btnRes.Visible = false;
            this.btnDelFor.Visible = false;
            this.rtbred.Visible = false;
            showNotes();
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHelp f2 = new FormHelp();
            f2.ShowDialog();
        }
        List<int> idList = new List<int>();
        bool StateofMenu;
        private void showNotes()
        {

            idList.Clear();
            this.listViewNoiDung.Items.Clear();
            List<Note> get = NoteControllers.getAllNotes();
            for (int i = (get.Count() - 1); i >= 0; i--)
            {
                Note note = get[i];
                if (note.Rac == false && note.PintoTop == true)
                {
                    ListViewItem show = new ListViewItem(note.TieuDe.Trim());
                    show.SubItems.Add(new ListViewItem.ListViewSubItem(show, "o"));
                    this.listViewNoiDung.Items.Add(show);
                    idList.Add(note.SoThuTu);
                }
            }
            for (int i = (get.Count() - 1); i >= 0; i--)
            {
                Note note = get[i];
                if (note.Rac == false && note.PintoTop == false)
                {
                    ListViewItem show2 = new ListViewItem(note.TieuDe.Trim());
                    show2.SubItems.Add(new ListViewItem.ListViewSubItem(show2, ""));
                    this.listViewNoiDung.Items.Add(show2);
                    idList.Add(note.SoThuTu);
                }
            }
        }

        private int DatTieuDe(string tieude)
        {
            for (int i = 0; i < tieude.Length; i++)
            {
                if (tieude[i].ToString() == "\n")
                {
                    return i;
                }
            }
            return tieude.Length;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.listViewNoiDung.SelectedIndices.Clear();
            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.txbSearch.Text = "";
            if (StateofMenu == true)
            {
                StateofMenu = false;
                this.cNoiDung.Text = "Ghi Chú Rác";
                this.btnNewNote.Visible = false;
                this.btnDelete.Visible = false;
                this.btnInfo.Visible = false;
                this.toolStrip3.Visible = false;
                this.rtbTypeNoiDung.ReadOnly = true;
                this.btnDelFor.Visible = true;
                this.btnRes.Visible = true;
                this.lbInfo.Text = "";
                this.rtbred.Visible = true;
                showRac();
            }
            else
            {
                StateofMenu = true;
                this.cNoiDung.Text = "Nội dung";
                this.btnNewNote.Visible = true;
                this.btnDelete.Visible = true;
                this.btnInfo.Visible = true;
                this.toolStrip3.Visible = true;
                this.rtbTypeNoiDung.ReadOnly = false;
                this.btnDelFor.Visible = false;
                this.btnRes.Visible = false;
                this.lbInfo.Text = "";
                this.rtbred.Visible = false;
                showNotes();
            }
        }

        private void btnNewNote_Click(object sender, EventArgs e)
        {
            if (StateofMenu == false)
                return;
            this.listViewNoiDung.SelectedIndices.Clear();
            this.lbInfo.Text = "";
            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.txbSearch.Text = "";
            Note getnote = new Note();
            getnote.SoThuTu = NoteControllers.getIdFromDb();
            getnote.TieuDe = "New note";
            getnote.NoiDung = "";
            getnote.ThongTin = DateTime.Now;
            getnote.Rac = false;
            getnote.PintoTop = false;

            NoteControllers.addNote(getnote);
            showNotes();

            int stt = 0;
            while (stt < idList.Count)
            {
                if (idList[stt] == getnote.SoThuTu)
                {
                    break;
                }
                stt++;
            }
            this.listViewNoiDung.Items[stt].Selected = true;
            this.listViewNoiDung.Select();
            this.rtbTypeNoiDung.Text = NoteControllers.getNote(getnote.SoThuTu).NoiDung;
            this.rtbTypeNoiDung.Focus();

        }
        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            this.panel1.BringToFront();
            if (this.lbInfo.Text == "")
            {
                int i = this.listViewNoiDung.SelectedItems[0].Index;
                int stt = idList[i];
                Note source = NoteControllers.getNote(stt);

                this.lbInfo.Text = "Modified: " + source.ThongTin.ToString().Trim();
            }
            else
            {
                this.lbInfo.Text = "";
                this.panel1.SendToBack();
            }
        }
        private void listViewNoiDung_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            this.lbInfo.Text = "";
            int i = this.listViewNoiDung.SelectedItems[0].Index;
            //int j = this.listViewNoiDung.SelectedItems[1].Index;
            int stt = idList[i];
            if (stt == -1)
            {
                this.listViewNoiDung.SelectedIndices.Clear();
                return;
            }
            Note source = NoteControllers.getNote(stt);
            this.rtbTypeNoiDung.Text = source.NoiDung;
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            if (source.Tags.Count <= 0) { return; }
            string showTags = "";
            foreach (Tag tag in source.Tags)
            {
                showTags = showTags + tag.TenTag + " ";
            }
            this.rtbShowTag.Text = showTags;
            this.rtbred.Text = showTags;
        }

        private void showRac()
        {
            idList.Clear();
            this.listViewNoiDung.Items.Clear();
            List<Note> get = NoteControllers.getAllNotes();
            for (int i = (get.Count() - 1); i >= 0; i--)
            {
                Note source = get[i];
                if (source.Rac == true)
                {
                    ListViewItem show = new ListViewItem(source.TieuDe.Trim());
                    show.SubItems.Add(new ListViewItem.ListViewSubItem(show, ""));
                    this.listViewNoiDung.Items.Add(show);
                    idList.Add(source.SoThuTu);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count == 0)
                return;

            int i = this.listViewNoiDung.SelectedItems[0].Index;
            int stt = idList[i];
            Note souce = NoteControllers.getNote(stt);
            souce.Rac = true;
            NoteControllers.updateNote(souce);
            showNotes();
            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.lbInfo.Text = "";
        }

        

        
        private void rtbTypeNoiDung_TextChanged(object sender, EventArgs e)
        {
            if (StateofMenu == true)
            {
                if (listViewNoiDung.SelectedItems.Count == 0)
                    return;

                int i = this.listViewNoiDung.SelectedItems[0].Index;

                //int j = this.listViewNoiDung.SelectedItems[1].Index;
                int stt = idList[i];
                Note source = NoteControllers.getNote(stt);

                string nd = source.NoiDung;

                source.NoiDung = this.rtbTypeNoiDung.Text.ToString();

               source.TieuDe = source.NoiDung.Substring(0, DatTieuDe(source.NoiDung));
                if (source.NoiDung != nd)
                {
                    this.lbInfo.Text = "";
                    source.ThongTin = DateTime.Now;
                }
                if (source.NoiDung == "")
                {
                    source.TieuDe = "New note";
                }

                NoteControllers.updateNote(source);

                this.listViewNoiDung.SelectedItems[0].Text = source.TieuDe.ToString();
            }
            else
            {
                return;
            }
        }

        private void txbSearch_TextChanged(object sender, EventArgs e)
        {

            if (this.txbSearch.Text == "")
            {
                if (StateofMenu == true)
                {
                    showNotes();
                }
                else
                {
                    showRac();
                }
                return;
            }
            this.listViewNoiDung.SelectedIndices.Clear();
            this.lbInfo.Text = "";
            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.listViewNoiDung.Items.Clear();
            List<Note> notes = NoteControllers.getAllNotesbyString(this.txbSearch.Text);
            List<Tag> tags = TagControllers.getAllTagbyString(this.txbSearch.Text);

            if (StateofMenu == true)
            {

                idList.Clear();
                ListViewItem timkiem = new ListViewItem("Tim kiem theo noi dung:");
                this.listViewNoiDung.Items.Add(timkiem);
                idList.Add(-1);


                foreach (Note get in notes)
                {
                    if (get.Rac == false && get.PintoTop == true)
                    {
                        ListViewItem duocpin = new ListViewItem(get.TieuDe.Trim());
                        duocpin.SubItems.Add(new ListViewItem.ListViewSubItem(duocpin, "o"));
                        this.listViewNoiDung.Items.Add(duocpin);
                        idList.Add(get.SoThuTu);
                    }
                }

                foreach (Note get in notes)
                {
                    if (get.Rac == false && get.PintoTop == false)
                    {
                        ListViewItem kduocpin = new ListViewItem(get.TieuDe.Trim());
                        kduocpin.SubItems.Add(new ListViewItem.ListViewSubItem(kduocpin, ""));
                        this.listViewNoiDung.Items.Add(kduocpin);
                        idList.Add(get.SoThuTu);
                    }
                }


                ListViewItem kiemtag = new ListViewItem("Tim kiem theo hashtag:");
                this.listViewNoiDung.Items.Add(kiemtag);
                idList.Add(-1);

                foreach (Tag source in tags)
                {

                    foreach (Note get in source.Notes)
                    {
                        if (get.Rac == false && get.PintoTop == true)
                        {
                            ListViewItem tagpin = new ListViewItem(get.TieuDe.Trim());
                            tagpin.SubItems.Add(new ListViewItem.ListViewSubItem(tagpin, "o"));
                            this.listViewNoiDung.Items.Add(tagpin);
                            idList.Add(get.SoThuTu);
                        }
                    }

                    foreach (Note get in source.Notes)
                    {
                        if (get.Rac == false && get.PintoTop == false)
                        {
                            ListViewItem tagnotpin = new ListViewItem(get.TieuDe.Trim());
                            tagnotpin.SubItems.Add(new ListViewItem.ListViewSubItem(tagnotpin, ""));
                            this.listViewNoiDung.Items.Add(tagnotpin);
                            idList.Add(get.SoThuTu);
                        }
                    }
                }
            }

            if (StateofMenu == false)
            {
                idList.Clear();

                ListViewItem nd = new ListViewItem("Tim kiem theo noi dung:");
                this.listViewNoiDung.Items.Add(nd);
                idList.Add(-1);

                foreach (Note get in notes)
                {
                    if (get.Rac == true)
                    {
                        ListViewItem searchnd = new ListViewItem(get.TieuDe.Trim());
                        searchnd.SubItems.Add(new ListViewItem.ListViewSubItem(searchnd, ""));
                        this.listViewNoiDung.Items.Add(searchnd);
                        idList.Add(get.SoThuTu);
                    }
                }


                ListViewItem tg = new ListViewItem("Tim kiem theo hashTag:");
                this.listViewNoiDung.Items.Add(tg);
                idList.Add(-1);
                foreach (Tag tag in tags)
                {
                    foreach (Note get in tag.Notes)
                    {
                        if (get.Rac == true)
                        {
                            ListViewItem searchtg = new ListViewItem(get.TieuDe.Trim());
                            searchtg.SubItems.Add(new ListViewItem.ListViewSubItem(searchtg, ""));
                            this.listViewNoiDung.Items.Add(searchtg);
                            idList.Add(get.SoThuTu);
                        }
                    }
                }
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (txbTypeTag.Text == "" )
            {
                return;
            }
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            int i = this.listViewNoiDung.SelectedItems[0].Index;
            //int j = this.listViewNoiDung.SelectedItems[1].Index;
            int stt = idList[i]++;
            Note source = NoteControllers.getNote(stt);
            foreach (Tag tag in source.Tags)
            {
                if (tag.TenTag == this.txbTypeTag.Text)
                {
                    source.Tags.Remove(tag);

                    Note get = new Note();
                    get.SoThuTu = source.SoThuTu;
                    get.TieuDe = source.TieuDe;
                    get.NoiDung = source.NoiDung;
                    get.ThongTin = source.ThongTin;
                    get.Rac = source.Rac;
                    get.PintoTop = source.PintoTop;
                    foreach (Tag getsource in source.Tags)
                    {
                        get.Tags.Add(getsource);
                    }

                    NoteControllers.deleteNote(source);
                    NoteControllers.addNote(get);

                    string showtag = "";
                    foreach (Tag gettag in get.Tags)
                    {
                        showtag = showtag + gettag.TenTag + " ";
                    }
                    this.rtbShowTag.Text = showtag;
                    this.rtbred.Text = showtag;

                    this.txbTypeTag.Text = "";

                    List<Tag> sourcetag = TagControllers.getAllTag();
                    foreach (Tag take in sourcetag)
                    {
                        if (take.Notes.Count() <= 0)
                        {
                            TagControllers.deleteTag(take);
                        }
                    }

                    return;
                }
            }
            return;
        }

        private void btnDelFor_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            int i = this.listViewNoiDung.SelectedItems[0].Index;
            //int j = this.listViewNoiDung.SelectedItems[1].Index;
            int stt = idList[i];
            Note getnote = NoteControllers.getNote(stt);

            NoteControllers.deleteNote(getnote);
            showRac();

            List<Tag> listtag = TagControllers.getAllTag();
            foreach (Tag gettag in listtag)
            {
                if (gettag.Notes.Count() <= 0)
                {
                    TagControllers.deleteTag(gettag);
                }
            }

            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.lbInfo.Text = "";
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            int i = this.listViewNoiDung.SelectedItems[0].Index;
            //int j = this.listViewNoiDung.SelectedItems[1].Index;
            int stt = idList[i];

            Note get = NoteControllers.getNote(stt);
            get.Rac = false;
            NoteControllers.updateNote(get);
            showRac();
            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.lbInfo.Text = "";
        }

        private void bunifuSwitch1_Click(object sender, EventArgs e)
        {
            if (this.listViewNoiDung.SelectedItems.Count <= 0)
                return;
            int i = this.listViewNoiDung.SelectedItems[0].Index;
            //int j = this.listViewNoiDung.SelectedItems[1].Index;
            int stt = idList[i];
            Note get = NoteControllers.getNote(stt);
            if (get.PintoTop == false)
            {
                get.PintoTop = true;
            }
            else
            {
                get.PintoTop = false;
            }

            NoteControllers.updateNote(get);
            showNotes();

            int x = 0;
            while (x < idList.Count)
            {
                if (idList[x] == get.SoThuTu)
                {
                    break;
                }
                x++;
            }
            this.listViewNoiDung.Items[x].Selected = true;
            this.listViewNoiDung.Select();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            panel1.SendToBack();
        }

        private void txbTypeTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                {

                    if (this.txbTypeTag.Text == "")
                    {
                        return;
                    }
                    if (this.listViewNoiDung.SelectedItems.Count <= 0)
                        return;

                    List<Tag> get = TagControllers.getAllTag();
                    Tag gettag = new Tag();
                    gettag.TenTag = this.txbTypeTag.Text;
                    TagControllers.addTag(gettag);

                    Tag tagforadd = TagControllers.getoneTag(this.txbTypeTag.Text);
                    int selectt = this.listViewNoiDung.SelectedItems[0].Index;
                    //int i2 = this.listViewNoiDung.SelectedItems[1].Index;
                    int chose = idList[selectt]++;
                    Note getchose = NoteControllers.getNote(chose);
                    getchose.Tags.Add(tagforadd);

                    Note temp = new Note();
                    temp.SoThuTu = getchose.SoThuTu;
                    temp.TieuDe = getchose.TieuDe;
                    temp.NoiDung = getchose.NoiDung;
                    temp.ThongTin = getchose.ThongTin;
                    temp.Rac = getchose.Rac;
                    temp.PintoTop = getchose.PintoTop;
                    foreach (Tag fina in getchose.Tags)
                    {
                        temp.Tags.Add(fina);
                    }

                    NoteControllers.deleteNote(getchose);
                    NoteControllers.addNote(getchose);

                    string show = "";
                    foreach (Tag forshow in getchose.Tags)
                    {
                        show = show + forshow.TenTag + " ";
                    }
                    this.rtbShowTag.Text = show;
                    this.rtbred.Text = show;

                    this.txbTypeTag.Text = "";

                }
            }
        }
        private void newNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StateofMenu == false)
                return;

            this.listViewNoiDung.SelectedIndices.Clear();

            this.lbInfo.Text = "";

            this.rtbTypeNoiDung.Clear();
            this.rtbShowTag.Clear();
            this.rtbred.Clear();
            this.txbSearch.Text = "";

            Note getnote = new Note();
            getnote.SoThuTu = NoteControllers.getIdFromDb();
            getnote.TieuDe = "New note";
            getnote.NoiDung = "";
            getnote.ThongTin = DateTime.Now;
            getnote.Rac = false;
            getnote.PintoTop = false;

            NoteControllers.addNote(getnote);
            showNotes();

            int stt = 0;
            while (stt < idList.Count)
            {
                if (idList[stt] == getnote.SoThuTu)
                {
                    break;
                }
                stt++;
            }

            this.listViewNoiDung.Items[stt].Selected = true;
            this.listViewNoiDung.Select();
            this.rtbTypeNoiDung.Text = NoteControllers.getNote(getnote.SoThuTu).NoiDung;
            this.rtbTypeNoiDung.Focus();

        }


    }
}
