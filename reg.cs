using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;


/*
https://regex101.com/
*/



public class mainWindow : Form
{
  int pad = 1;
  int terminate = 500;
  TableLayoutPanel tlp = new TableLayoutPanel() { Dock = DockStyle.Fill };
  Button bt = new Button() { Text = "検索" };
  TextBox sb = new TextBox() { Height = 20, Dock = DockStyle.Fill };
  Panel p = new Panel() { Height = 25, Dock = DockStyle.Fill };
  TableLayoutPanel stlp = new TableLayoutPanel() { ColumnCount = 2, RowCount = 1, Dock = DockStyle.Fill };
  TextBox ab = new TextBox() { Height = 20, Dock = DockStyle.Fill };
  TextBox bb = new TextBox() { Height = 20, Dock = DockStyle.Fill };
  RichTextBox rb = new RichTextBox() { Height = 20, Dock = DockStyle.Fill };
  RichTextBox tb = new RichTextBox() { Dock = DockStyle.Fill, Multiline = true };
  public static void Main()
  {
    Application.Run(new mainWindow());
  }

  mainWindow()
  {
    this.Height = 500;
    this.Width = 500;
    this.Text = "選択した文字列にヒットする正規表現を作成するやつ";

    this.Controls.Add(tlp);
    tlp.Controls.Add(bt);
    // tlp.Controls.Add(sb);
    tlp.Controls.Add(p);
    p.Controls.Add(stlp);
    stlp.Controls.Add(bb);
    stlp.Controls.Add(ab);
    tlp.Controls.Add(rb);
    tlp.Controls.Add(tb);


    tb.Text = body;
    bt.Click += (sender, e) => FUN();
  }

  private void FUN()
  {
    int padding = pad, apadding = pad, bpadding = pad;
    string q = "";
    MatchCollection finds = Regex.Matches("Using this text is not a good idea.Using this text is not a good idea.", "Using this text is not a good idea.");


    // 文字列の選択位置を取得する
    string word = Regex.Escape(tb.SelectedText);
    int start = tb.SelectionStart;
    int end = tb.SelectionStart + tb.SelectionLength;



    // (.*?)に検索ワードがヒットしていない場合 または 検索絵結果が一意に決まっていない場合に
    while ((Regex.Match(tb.Text, q).Groups[1].Value != Regex.Unescape(word)) || (finds.Count != 1))
    {
      // 選択した文字の前後[padding]文字を取得する
      bb.Text = BAround(tb.Text, word, start, bpadding);
      ab.Text = AAround(tb.Text, word, end, apadding);


      // 正規表現を作成し検索
      q = Regex.Escape(bb.Text) + "(.*?)" + Regex.Escape(ab.Text);
      finds = SearchText(tb.Text, q);

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

    tb.Select(CurrentText(finds, 0), q.Length);
    tb.Focus();
    q = q.Replace("/", "\\/");
    rb.Text = q;
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

  private MatchCollection SearchText(string target, string keyword)
  {
    if (keyword.Length == 0) { return null; }

    MatchCollection finds = Regex.Matches(target, keyword);
    return finds;
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
    tlp.Controls.Add(bt);
    // tlp.Controls.Add(sb);
    tlp.Controls.Add(p);
    p.Controls.Add(stlp);
    stlp.Controls.Add(bb);
    stlp.Controls.Add(ab);
    tlp.Controls.Add(rb);
    tlp.Controls.Add(tb);


    tb.Text = body;
    bt.Click += (sender, e) => FUN();
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
}