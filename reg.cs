using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;


/*
https://regex101.com/
*/


namespace UTK
{

  public class RegForm : Form
  {
    int pad = 1;
    int padding = 1;
    int apadding = 1;
    int bpadding = 1;
    int terminate = 500;
    int count = 0;
    string q = "";
    MatchCollection collection = null;
    TableLayoutPanel tlp = new TableLayoutPanel() { Dock = DockStyle.Fill };
    TableLayoutPanel ltlp = new TableLayoutPanel() { Height = 90, ColumnCount = 2, RowCount = 1, Dock = DockStyle.Fill };
    TableLayoutPanel sltlp = new TableLayoutPanel() { Width = 150, ColumnCount = 3, RowCount = 2, };
    Button sbt = new Button() { Text = "検索", BackColor = Color.Gray, };
    Button ubt = new Button() { Text = "▲", Width = 25, BackColor = Color.Gray, };
    Button dbt = new Button() { Text = "▼", Width = 25, BackColor = Color.Gray, };
    TextBox sb = new TextBox() { Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };
    Button gbt = new Button() { Text = "生成" };
    TableLayoutPanel stlp = new TableLayoutPanel() { Height = 25, ColumnCount = 2, RowCount = 1, BackColor = Color.Red, Dock = DockStyle.Fill };
    TextBox ab = new TextBox() { Height = 20, ReadOnly = true };
    TextBox bb = new TextBox() { Height = 20, ReadOnly = true };
    TextBox rb = new TextBox() { WordWrap = false, Multiline = false, Height = 20, Dock = DockStyle.Fill };
    TextBox tb = new TextBox() { Dock = DockStyle.Fill, Multiline = true, Font = new Font("BIZ UDゴシック", 10), ScrollBars = ScrollBars.Vertical };

    public RegForm()
    {
      collection = null;

      InitializeFormSettings();
      InitializeComponents();
      ConfigureEventHandlers();
    }
    private void InitializeFormSettings()
    {
      this.Height = 500;
      this.Width = 1000;
      this.Text = "選択した文字列にヒットする正規表現を作成するやつ";
    }

    private void InitializeComponents()
    {
      AddControlsToLayout();
      InitializeControls();
    }
    private void AddControlsToLayout()
    {
      this.Controls.Add(tlp);
      tlp.Controls.Add(ltlp);
      ltlp.Controls.Add(sltlp);
      sltlp.Controls.Add(sbt);
      sltlp.Controls.Add(dbt);
      sltlp.Controls.Add(ubt);
      ltlp.Controls.Add(sb);
      tlp.Controls.Add(gbt);
      tlp.Controls.Add(stlp);
      stlp.Controls.Add(bb);
      stlp.Controls.Add(ab);
      tlp.Controls.Add(rb);
      tlp.Controls.Add(tb);
    }
    private void InitializeControls()
    {
      tb.Font = new Font("BIZ UDゴシック", 10);
      sb.Font = new Font("BIZ UDゴシック", 10);
      ab.Width = -10 + stlp.Width / 2;
      bb.Width = -10 + stlp.Width / 2;
      sb.Width = -160 + ltlp.Width;
      tb.Text = body;
    }

    private void ConfigureEventHandlers()
    {
      gbt.Click += (sender, e) => FUN();
      rb.DoubleClick += (sender, e) => rb.Select(0, rb.Text.Length);

      // search Events
      sb.TextChanged += (sender, e) => initializeSB();
      sbt.Click += (sender, e) => GetCollection();

      ubt.Click += (sender, e) => MoveCollection(-1);
      dbt.Click += (sender, e) => MoveCollection(1);

      //ショートカット関連の操作
      this.KeyPreview = true;
      this.KeyDown += (sender, e) => SuppressShortcut(e);
      this.KeyDown += (sender, e) => SetShortcut(e);

      this.Resize += (sender, e) =>
      {
        ab.Width = -10 + stlp.Width / 2;
        bb.Width = -10 + stlp.Width / 2;
        sb.Width = -160 + ltlp.Width;
      };
    }

    private void FUN()
    {
      MatchCollection finds = Regex.Matches("Using this text is not a good idea.Using this text is not a good idea.", "Using this text is not a good idea.");

      // 文字列の選択位置を取得する
      string word = Regex.Escape(tb.SelectedText);
      int start = tb.SelectionStart;
      int end = tb.SelectionStart + tb.SelectionLength;

      GenerateRegex(finds, word, start, end);

      // tb.Select(CurrentText(finds, 0), q.Length);
      if (padding <= terminate)
      {
        tb.Select(start, finds[0].Groups[1].Value.Length);
        tb.Focus();
      }
      q = q.Replace("/", "\\/");
      ab.Text = Regex.Escape(ab.Text);
      bb.Text = Regex.Escape(bb.Text);
      rb.Text = q;
    }

    private void GenerateRegex(MatchCollection finds, string word, int start, int end)
    {
      // (.*?)に検索ワードがヒットしていない場合 または 検索絵結果が一意に決まっていない場合に
      while ((Regex.Match(tb.Text, q).Groups[1].Value != Regex.Unescape(word)) || (finds.Count != 1))
      {
        // 選択した文字の前後[padding]文字を取得する
        bb.Text = BAround(tb.Text, word, start, bpadding);
        ab.Text = AAround(tb.Text, word, end, apadding);


        // 正規表現を作成し検索
        q = Regex.Escape(bb.Text) + "(.*?)" + Regex.Escape(ab.Text);
        finds = SearchText(tb.Text, q, false);

        // 一意に絞り切れない場合に検索を打ち切る
        if (padding > terminate)
        {
          rb.BackColor = Color.Yellow;
          break;
        }
        else
        {
          rb.BackColor = Color.White;
        }


        // Console.WriteLine("b: " + bpadding + "  a: " + apadding + "  p: " + padding);
        // Console.WriteLine("selected text: "+ word);
        // Console.WriteLine("reg Text: " + q + " find count " + finds.Count);
        // Console.WriteLine("グループの結果: " + Regex.Match(tb.Text, q).Groups[1].Value + " unescape: " + Regex.Unescape(word));
        // Console.WriteLine("ヒットしたテキストの中に選択した文字列がヒットする数-->: " + Regex.Matches(Regex.Match(tb.Text, q).Value, word).Count);

        if (bpadding > apadding)
        {
          bpadding--;
          apadding++;
        }
        else if (bpadding < apadding)
        {
          bpadding++;
        }
        else
        {
          bpadding++;
        }
        ++padding;


        // if ((finds.Count == 1)) { Console.WriteLine("出る条件1"); }
        // if ((Regex.Match(tb.Text, q).Groups[1].Value == Regex.Unescape(word))) { Console.WriteLine("出る条件2"); }
      }
    }


    private string BAround(string target, string keyword, int caret, int padding)
    {
      int start = Math.Max(0, caret - padding); // 範囲外防止
      int length = Math.Min(padding, caret);   // 実際の長さ調整
      return target.Substring(start, length);
    }
    private string AAround(string target, string keyword, int caret, int padding)
    {
      int length = Math.Min(padding, target.Length - caret); // 範囲外防止
      return target.Substring(caret, length);
    }

    // 任意の一単語を特定する正規表現を生成する。
    private string CreateReg(string target, string keyword)
    {
      return Regex.Escape(BAround(target, keyword, target.IndexOf(keyword), pad)) + "(.*?)" + Regex.Escape(AAround(target, keyword, target.IndexOf(keyword), pad));
    }

    private void MoveCollection(int diff)
    {
      if (collection == null)
      {
        collection = SearchText(tb.Text, sb.Text, true);
        sb.BackColor = Color.White;
        if (collection == null || collection.Count == 0) sb.BackColor = Color.Yellow;
        // HighlightText(tb, sbt, sb.Text, collection, times += diff);
      }
      else
      {
        tb.BackColor = Color.White;
        HighlightText(tb, sbt, sb.Text, collection, count += diff);
      }
    }

    private void GetCollection()
    {
      collection = SearchText(tb.Text, (sb.Text), true);
      if (collection == null || collection.Count == 0)
      {
        sb.BackColor = Color.Yellow;
      }
      else
      {
        sb.BackColor = Color.White;
      }
      count = 0;
    }

    private MatchCollection SearchText(string target, string keyword, bool isHigh = true)
    {
      if (keyword.Length == 0) { return null; }
      MatchCollection finds = Regex.Matches(target, keyword);
      if (isHigh) HighlightText(tb, sbt, keyword, finds, 0);
      return finds;
    }


    private void HighlightText(TextBox tb, Button current, string key, MatchCollection indexes, int times)
    {
      if (indexes.Count == 0) { return; }

      int t = (((times % indexes.Count) + indexes.Count) % indexes.Count);
      tb.Select(indexes[t].Index, indexes[t].Value.Length);
      tb.Focus();
      tb.ScrollToCaret();

      if ((((times + 1 % indexes.Count) + indexes.Count) % indexes.Count) == 0)
      {
        current.Text = "検索" + indexes.Count + "/" + indexes.Count;
      }
      else
      {
        int c = (((times + 1 % indexes.Count) + indexes.Count) % indexes.Count);
        if (c < 0) c *= -1;
        current.Text = "検索" + c + "/" + indexes.Count;
      }

    }

    private void SetShortcut(KeyEventArgs e)
    {
      // ショートカット設定
      if (e.KeyData == (Keys.Control | Keys.F))
      {
        sb.Focus();
      }
      else if ((e.KeyData == (Keys.Control | Keys.N)) || (e.KeyData == (Keys.Control | Keys.S))
                || (e.KeyData == (Keys.Alt | Keys.Down)) || (e.KeyData == (Keys.Alt | Keys.Enter))
                )
      {
        // 次を検索 c-n c-s a-下矢印
        MoveCollection(1);
      }
      else if ((e.KeyData == (Keys.Shift | Keys.Control | Keys.N)) || (e.KeyData == (Keys.Control | Keys.W)) || (e.KeyData == (Keys.Alt | Keys.Up)) || (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Enter)))
      {
        // tb.SelectionColor = Color.Red;
        MoveCollection(-1);
      }
      else if (e.KeyData == (Keys.Control | Keys.A))
      {
        if (tb.Focused)
        {
          tb.Select(0, tb.Text.Length);
        }
        if (sb.Focused)
        {
          sb.Select(0, sb.Text.Length);
        }
        if (rb.Focused)
        {
          rb.Select(0, rb.Text.Length);
        }
      }
    }

    private void SuppressShortcut(KeyEventArgs e)
    {
      if ((e.KeyCode == (Keys.Control | Keys.I)) || (e.KeyCode == (Keys.Control | Keys.E)) || (e.KeyCode == (Keys.Control | Keys.B)) || (e.KeyCode == (Keys.Control | Keys.R)) || (e.KeyCode == (Keys.Control | Keys.L)))
      {
        e.SuppressKeyPress = true;
      }
    }

    private void initializeSB()
    {
      collection = null;
      count = 0;
      sbt.Text = "検索";
    }
    private int CurrentText(MatchCollection indexes, int times)
    {
      if (indexes.Count == 0) { return 0; }

      return indexes[(((times % indexes.Count) + indexes.Count) % indexes.Count)].Index;
    }

    string body =
    @"
  mainWindow()
  {
    this.Height = 500;
    this.Width = 500;
    this.Text = 選択した文字列にヒットする正規表現を作成するやつ;

    this.Controls.Add(tlp);
    tlp.Controls.Add(gbt);
    // tlp.Controls.Add(sb);
    tlp.Controls.Add(p);
    p.Controls.Add(stlp);
    stlp.Controls.Add(bb);
    stlp.Controls.Add(ab);
    tlp.Controls.Add(rb);
    tlp.Controls.Add(tb);


    tb.Text = body;
    gbt.Click += (sender, e) => FUN();
  }

  private void FUN()
  {
    int padding = pad, apadding = pad, bpadding = pad;
    string q = ;
    MatchCollection finds = Regex.Matches(Using this text is not a good idea.Using this text is not a good idea., Using this text is not a good idea.);


    // 文字列の選択位置を取得する
    string word = Regex.Escape(tb.SelectedText);
    int start = tb.SelectionStart;
    int end = tb.SelectionStart + tb.SelectionLength;

    Console.WriteLine(do);


    // (.*?)に検索ワードがヒットしていない場合 または 検索絵結果が一意に決まっていない場合に
    while ((Regex.Match(tb.Text, q).Groups[1].Value != Regex.Unescape(word)) || (finds.Count != 1))
    {
      // 選択した文字の前後[padding]文字を取得する
      bb.Text = BAround(tb.Text, word, start, bpadding);
      ab.Text = AAround(tb.Text, word, end, apadding);


      // 正規表現を作成し検索
      q = Regex.Escape(bb.Text) + (.*?) + Regex.Escape(ab.Text);
      finds = SearchText(tb.Text, q);

      // 一意に絞り切れない場合に検索を打ち切る
      if (padding > terminate)
      {
        rb.BackColor = Color.Yellow;
        break;
      }
";

    public static void Main()
    {
      Application.Run(new RegForm());
    }
  }
}
