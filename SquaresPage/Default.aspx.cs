using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace SquaresPage
{
	public partial class Default : System.Web.UI.Page
	{
		static List<Point> pts = new List<Point>();
		static List<List<Point>> llp = new List<List<Point>>();
		const string fileFilter = "Point list file (*.pntlst)|*.pntlst";
		static int currPage = 0;
		static int ptsPerPage = 5;

		// auxiliary methods

		int? IsInt(string str)
		{
			int i=0;
			if (int.TryParse (str, out i))
				return i;
			else
				return null;
		}

		void EliminateRepeats()
		{
			if (pts.Count < 2)
				return;
			for (int i = 1; i < pts.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (pts[i] == pts[j])
						pts.RemoveAt(i);
				}
			}
		}

		public void RegenPointPages()
		{
			ptsPerPage = IsInt(dropPages.SelectedValue).Value;
			llp = new List<List<Point>>();
			int listIdx = -1;
			for (int i = 0; i < pts.Count; i++)
			{
				if (i % ptsPerPage == 0)
				{
					llp.Add(new List<Point>());
					listIdx++;
				}
				llp[listIdx].Add(pts[i]);
			}
		}

		public void ApplyCurrentPage()
		{
			lblNofN.Text="Page "+(currPage+1).ToString()+" of "+llp.Count.ToString();

			listOfPoints.Text = "Currently the following points are added:<br>";

			string pntLstStr = "";
			for (int i = 0; i < llp[currPage].Count; i++)
			{
				pntLstStr += (currPage*ptsPerPage + i + 1).ToString() + ". ";
				pntLstStr += llp[currPage][i].CoordsToString(true);
				pntLstStr += "<br>";
			}
			listOfPoints.Text += pntLstStr;
		}

		void Refresh()
		{
			EliminateRepeats();
			RegenPointPages();
			bool ptsPresent = pts.Count > 0;

			btnExportList.Enabled = ptsPresent;
			dropPages.Enabled = ptsPresent;

			btnCountSq.Enabled = pts.Count > 3;

			if (ptsPresent)
			{
				ApplyCurrentPage();

//				string pntLstStr = "Currently the following points are added:<br>";
//				for (int i = 0; i < pts.Count; i++)
//				{
//					pntLstStr += (i + 1).ToString() + ". ";
//					pntLstStr += pts[i].CoordsToString(true);
//					pntLstStr += "<br>";
//				}
//				listOfPoints.Text = pntLstStr;
			}
			else
				listOfPoints.Text = "";

			listOfSquares.Text = "";

			if (pts.Count > ptsPerPage)
			{
				btnRight.Enabled = currPage < llp.Count-1;
				btnLeft.Enabled = currPage > 0;
			}
			else
			{
				btnRight.Enabled = false;
				btnLeft.Enabled = false;
			}
		}

		public void importFileList(bool del)
		{
			System.IO.Stream fileList = null;
			OpenFileDialog importListDialog = new OpenFileDialog();
			importListDialog.Filter = fileFilter;
			importListDialog.RestoreDirectory = true ;

			if(importListDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if ((fileList = importListDialog.OpenFile()) != null)
					{
						List<Point> addedPts=new List<Point>();

						StreamReader sr = new StreamReader(fileList);
						String line=null;
						while(true)
						{
							line = sr.ReadLine();
							if(line == null) break;
							string[] twoNum = line.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);

							string errortext="<script>alert('Incorrect data format in file. No action was carried out.');</script>";

							if(twoNum.Length!=2)
							{
								Response.Write (errortext);
								return;
							}
							int? x=IsInt(twoNum[0]);
							int? y=IsInt(twoNum[1]);
							if(x==null || y==null)
							{
								Response.Write (errortext);
								return;
							}

							addedPts.Add(new Point(x.Value, y.Value));
						}

						if(del)
						{
							// ezt csinaljuk csak akkor, ha minden tuti, sikerult beolvasni a fajlt
							pts.Clear();
						}

						pts.AddRange(addedPts);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
				return;
			}
		}

		// btnClicked methods

		public void btnAddPtClicked(object sender, EventArgs args)
		{
			int? Xc=IsInt(XCoord.Value);
			int? Yc=IsInt(YCoord.Value);

			if (Xc.HasValue && Yc.HasValue)
			{
				Point newp = new Point(Xc.Value, Yc.Value);
				bool notyet = true; // was not requested, but makes no sense to store identical points
				for (int i = 0; i < pts.Count; i++)
				{
					if (pts[i] == newp)
					{
						Response.Write ("<script>alert('This point already exists in the list, it will not be added again.');</script>");
						notyet = false;
						break;
					}
				}

				XCoord.Value = "";
				YCoord.Value = "";

				if (notyet)
					pts.Add(newp);
			}
			else
			{
				Response.Write ("<script>alert('Please provide an integer numbers as coordinates!');</script>");
				if (!Xc.HasValue)
					XCoord.Value = "";
				if (!Yc.HasValue)
					YCoord.Value = "";
			}
			Refresh();
		}

		public void btnClearListClicked(object sender, EventArgs args)
		{
			pts.Clear();
			Refresh();
		}

		public void btnRemovePtsClicked(object sender, EventArgs args)
		{
			string[] idxsStr = toBeRemoved.Value.Split(new char[] {' ','\t', ','}, StringSplitOptions.RemoveEmptyEntries);
			List<int> idxs = new List<int>();
			for (int i = 0; i < idxsStr.Length; i++)
			{
				int? num=IsInt(idxsStr[i]);
				if (num == null)
				{
					Response.Write("<script>alert('Invalid list!');</script>");
					toBeRemoved.Value = "";
					return;
				}
				else
					idxs.Add(num.Value);
			}
			idxs.Sort();
			for (int i = idxs.Count-1; i >= 0; i--)
			{
				if (idxs[i] > pts.Count || idxs[i] < 1)
					Response.Write("<script>alert('There is no point " + idxs[i].ToString() + ", it will not be deleted.');</script>");
				else
					pts.RemoveAt(idxs[i] - 1);
			}
			toBeRemoved.Value = "";
			Refresh();
		}

		public void btnImportListClicked(object sender, EventArgs args)
		{
			DialogResult result = MessageBox.Show("Do you want the existing points to be removed? " +
				"If not, points from the file will be added to the already existing points.",
				"Delete existing points?",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);

			if (result == DialogResult.Yes)
				importFileList(true);
			else if (result == DialogResult.No)
				importFileList(false);
			Refresh();
			return;
		}

		public void btnExportListClicked(object sender, EventArgs args)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Export points to file";
			sfd.Filter = fileFilter;
			DialogResult dlgRes = sfd.ShowDialog();
			if (dlgRes == DialogResult.OK && sfd.FileName != "")
			{
				System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
				StreamWriter sw = new StreamWriter(fs);
				sw.BaseStream.Seek(0, SeekOrigin.End);

				for (int i = 0; i < pts.Count; i++)
					sw.Write(pts[i].CoordsToString()+"\n");

				sw.Close();
			}
		}

		public void btnDeleteListClicked(object sender, EventArgs args)
		{
			FileDialog fd = new OpenFileDialog();
			fd.Filter = fileFilter;
			fd.Title = "Delete";
			DialogResult dlgRes = fd.ShowDialog();
			if (dlgRes == DialogResult.OK)
			{
				string fullPath = Path.Combine(fd.InitialDirectory, fd.FileName);
				if (File.Exists(@fullPath))
				{
					try
					{
						File.Delete(@fullPath);
					}
					catch (IOException e)
					{
						Console.WriteLine(e.Message);
						return;
					}
				}
			}
		}

		public void btnCountSqClicked(object sender, EventArgs args)
		{
			int countSq = 0;
			Qdrs qts = new Qdrs(pts);
			for (int i = 0; i < qts.QS.Count; i++)
			{
				if (qts.QS[i].IsSquare())
				{
					countSq++;
					if (countSq == 1)
					{
						listOfSquares.Text = "Squares are the following:<br>";
					}
					listOfSquares.Text += countSq.ToString() + ". " + qts.QS[i].GetPointsText()+"<br>";
				}
			}
			string msg = "Number of squares: " + countSq;
			Response.Write ("<script>alert('" +msg+ "');</script>");
		}

		public void DropPagesChanged(object sender, EventArgs args)
		{
			ptsPerPage = IsInt(dropPages.SelectedValue).Value;
			currPage = 0;
			Refresh();
		}

		public void btnLeftClicked(object sender, EventArgs args)
		{
			if (currPage > 0)
				currPage--;
			btnLeft.Enabled = currPage > 0;
			ApplyCurrentPage();
			btnRight.Enabled = true;
		}

		public void btnRightClicked(object sender, EventArgs args)
		{
			if (currPage < llp.Count - 1)
				currPage++;
			btnRight.Enabled = currPage < llp.Count - 1;
			ApplyCurrentPage();
			btnLeft.Enabled = true;
		}

		public void ChangeSort(object sender, EventArgs args)
		{
			if (dropAscDesc.SelectedValue == "d")
			{
				if (dropSortBy.SelectedValue == "x")
					pts = pts.OrderByDescending(p => p.X).ToList();
				else
					pts = pts.OrderByDescending(p => p.Y).ToList();
			}
			else
			{
				if (dropSortBy.SelectedValue == "x")
					pts = pts.OrderBy(p => p.X).ToList();
				else
					pts = pts.OrderBy(p => p.Y).ToList();
			}
			Refresh();
		}
	}
}
