﻿using System;
using ClassLibraryWork;
using System.Windows.Forms;
using WindowsFormsControlLibraryJob;
using ClassLibraryWork.Serialization;

namespace _4_2
{
    public partial class MainMenu : Form
    {

        private readonly Human _human = Human.Instance; 
        readonly FormEmployee _formEmployee      = new FormEmployee(); 
        readonly FormTypeOfWork _formTypeOfWork  = new FormTypeOfWork(); 
        readonly FormJob _formJob                = new FormJob();

        public MainMenu()
        {
            InitializeComponent();
            _human.EmployeeAdded     += _human_EmployeeAdded;
            _human.EmployeeRemoved   += _human_EmployeeRemoved;

            _human.TypeOfWorkAdded   += _human_TypeOfWorkAdded;
            _human.TypeOfWorkRemoved += _human_TypeOfWorkRemoved;

            _human.JobAdded          += _human_JobAdded;
            _human.JobRemoved        += _human_JobRemoved;

        }

        private void _human_JobRemoved(object sender, EventArgs e)
        {
            var job = sender as Job;
            for (int i = 0; i < tabPageJob.Controls.Count; i++)
            {
                if ((tabPageJob.Controls[i] as UserControlJob)?.Job == job)
                {
                    tabPageJob.Controls.RemoveAt(i);
                    break;
                }
            }

        }

        private void _human_TypeOfWorkRemoved(object sender, EventArgs e)
        {
            var typeOfWorkId = (int)sender;
            for (int i = 0; i < listViewTypeOfWork.Items.Count; i++)
            {
                if(((TypeOfWork)listViewTypeOfWork.Items[i].Tag).TypeOfWorkId == typeOfWorkId)
                {
                    listViewTypeOfWork.Items.RemoveAt(i); 
                    break;
                }
            }
        }

        private void _human_EmployeeRemoved(object sender, EventArgs e)
        {
            var employeeId = (int)sender;
            for (int i = 0; i < listViewEmployee.Items.Count; i++)
            {
                if(((Employee)listViewEmployee.Items[i].Tag).EmployeeId == employeeId)
                {
                    listViewEmployee.Items.RemoveAt(i); 
                    break;
                }
            }
        }

        private void _human_JobAdded(object sender, EventArgs e)
        {
            var job = sender as Job;
            if (job != null)
            {
                UserControlJob userControl = new UserControlJob(job)
                {
                    Dock = DockStyle.Top
                }; tabPageJob.Controls.Add(userControl);

            }
        }

        private void _human_TypeOfWorkAdded(object sender, EventArgs e)
        {
            var typeOfWork = sender as TypeOfWork;
            if (typeOfWork != null)
            {
                var listViewItem = new ListViewItem
                {
                    Tag = typeOfWork,
                    Text = typeOfWork.TypeOfWorkId.ToString()
                };
                listViewItem.SubItems.Add(typeOfWork.PaymentPerDay.ToString());
                listViewItem.SubItems.Add(typeOfWork.Description.ToString());
                listViewTypeOfWork.Items.Add(listViewItem);
            }
        }

        private void _human_EmployeeAdded(object sender, EventArgs e)
        {
            var employee = sender as Employee;
            if (employee != null)
            {
                var listViewItem = new ListViewItem
                {
                    Tag = employee,
                    Text = employee.EmployeeId.ToString()
                };

                listViewItem.SubItems.Add(employee.ToString());
                listViewItem.SubItems.Add(employee.Salary.ToString());
                listViewEmployee.Items.Add(listViewItem);
            }
        }
 
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var employee = new Employee();
            _formEmployee.Employee = employee;
            if (_formEmployee.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _human.AddEmployee(employee);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var employee = listViewEmployee.SelectedItems[0].Tag as Employee; 
                _formEmployee.Employee = employee;
                if (_formEmployee.ShowDialog() == DialogResult.OK)
                {
                    listViewEmployee.SelectedItems[0].Text = _formEmployee.Employee.EmployeeId.ToString();
                    listViewEmployee.SelectedItems[0].SubItems[1].Text = _formEmployee.Employee.ToString();
                    listViewEmployee.SelectedItems[0].SubItems[2].Text = _formEmployee.Employee.Salary.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не выбрана строка с сотрудником");
            }
        }


        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var typeOfWork = new TypeOfWork();
            _formTypeOfWork.TypeOfWork = typeOfWork;
            if (_formTypeOfWork.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _human.AddTypeOfWork(typeOfWork);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var typeOfWork = listViewTypeOfWork.SelectedItems[0].Tag as TypeOfWork;
                _formTypeOfWork.TypeOfWork = typeOfWork;
                if (_formTypeOfWork.ShowDialog() == DialogResult.OK)
                {
                    listViewTypeOfWork.SelectedItems[0].Text = _formTypeOfWork.TypeOfWork.TypeOfWorkId.ToString();
                    listViewTypeOfWork.SelectedItems[0].SubItems[1].Text = _formTypeOfWork.TypeOfWork.PaymentPerDay.ToString();
                    listViewTypeOfWork.SelectedItems[0].SubItems[2].Text = _formTypeOfWork.TypeOfWork.Description.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не выбрана строка с видом работы");
            }
        }

        private void addToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var job = new Job();
            _formJob.Job = job; 
            if (_formJob.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _human.AddJob(job);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void editToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < tabPageJob.Controls.Count; i++)
                {
                    var userControl = tabPageJob.Controls[i] as UserControlJob;
                    if (userControl != null)
                    {
                        if (userControl.Selected)
                        {
                            var job = userControl.Job;
                            _formJob.Job = job;
                            if (_formJob.ShowDialog() == DialogResult.OK)
                            {
                                userControl.Refresh();
                            }
                            break;
                        }
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Не выбрана строка с работой");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listViewEmployee_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    var employee = listViewEmployee.SelectedItems[0].Tag as Employee;
                    if (employee != null)
                    {
                        _human.RemoveEmployee(employee.EmployeeId);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не выбрана строка с сотрудником");
                }
            }
        }

        private void listViewJob_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    try
            //    {
            //        var Job = listViewJob.SelectedItems[0].Tag as Job;
            //        if (Job != null)
            //        {
            //            _human.RemoveJob(Job);
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("Не выбрана строка с работой");
            //    }
            //}
        }

        private void listViewTypeOfWork_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    var typeOfWork = listViewTypeOfWork.SelectedItems[0].Tag as TypeOfWork;
                    if (typeOfWork != null)
                    {
                        _human.RemoveTypeOfWork(typeOfWork.TypeOfWorkId);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не выбрана строка с видом работы");
                }
            }
        }

        private void exportXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogMain.Filter = "XML-файлы|*.xml|Все файлы|*.*";
            if (saveFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Save(saveFileDialogMain.FileName, SerializeType.XML);
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogMain.Filter = "JSON-файлы|*.json|Все файлы|*.*";
            if (saveFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Save(saveFileDialogMain.FileName, SerializeType.JSON);
            }
        }

        private void exportBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogMain.Filter = "Двоичные файлы|*.bin|Все файлы|*.*";
            if (saveFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Save(saveFileDialogMain.FileName, SerializeType.Binary);
            }
        }

        private void importXMLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialogMain.Filter = "XML-файлы|*.xml|Все файлы|*.*";
            if (openFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Load(openFileDialogMain.FileName, SerializeType.XML);

            }
        }

        private void importJSONToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialogMain.Filter = "JSON-файлы|*.json|Все файлы|*.*";
            if (openFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Load(openFileDialogMain.FileName, SerializeType.JSON);
            }
        }

        private void importBinaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialogMain.Filter = "Двоичные файлы|*.bin|Все файлы|*.*";
            if (openFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                JobServiceSerializable.Load(openFileDialogMain.FileName, SerializeType.Binary);
            }
        }
    }
}
