using Azure;
using CIRAP.Models;
using CIRAP.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using ProjectInfo = CIRAP.Models.ProjectInfo;
using System.Net;
using System.Net.Mail;
using System.Diagnostics.Metrics;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CIRAP.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectInfoContext? _ProjectInfoContext;
        private readonly sys_SubProjectContext? _sysSubProjectContext;
        private readonly ProjectTasksContext? _ProjectTasksContext;
        private readonly Sys_Task1StepContext? _sysTask1StepContext;
        private readonly Sys_Task2StepContext? _sysTask2StepContext;
        private readonly UserContext? _UserContext;
        private readonly RiskAssessContext? _RiskAssessContext;
        private readonly ProjectTasksRiskAssessContext? _ProjectTasksRiskAssessContext;
        private readonly RiskAssessCMpersonContext? _RiskAssessCMpersonContext;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _ProjectInfoContext = new ProjectInfoContext();
            _sysSubProjectContext = new sys_SubProjectContext();
            _ProjectTasksContext = new ProjectTasksContext();
            _sysTask1StepContext = new Sys_Task1StepContext();
            _sysTask2StepContext = new Sys_Task2StepContext();
            _UserContext = new UserContext();
            _RiskAssessContext = new RiskAssessContext();
            _ProjectTasksRiskAssessContext = new ProjectTasksRiskAssessContext();
            _RiskAssessCMpersonContext = new RiskAssessCMpersonContext();
        }
        public JsonResult setRiskAssessCMname(string projectID, string cmName)
        {
            var t = new RiskAssessCMperson
            {

                Project_ID = projectID,
                CM_Name = cmName
            };
            _RiskAssessCMpersonContext?.Add(t);
            _RiskAssessCMpersonContext.SaveChanges();
            List<RiskAssessCMperson>? raList = _RiskAssessCMpersonContext?.RiskAssessCMperson.Where(m => m.Project_ID.Equals(projectID)).ToList();
            return Json(raList);
        }
        public JsonResult getRiskAssessCMname(string projectID)
        {
            List<RiskAssessCMperson>? raList = _RiskAssessCMpersonContext?.RiskAssessCMperson.Where(m => m.Project_ID.Equals(projectID)).ToList();
            return Json(raList);
        }

        public IActionResult TreeView(string projectID)
        {
            List<sys_SubProject>? sysSubPJList = _sysSubProjectContext?.sys_SubProject.ToList();
            ViewBag.projectID = projectID;
            List<ProjectTasks>? ptList = _ProjectTasksContext?.ProjectTasks.Where(m => m.Project_ID.Equals(projectID)).OrderBy(m => m.SubProjectID).ToList();
            return View(ptList);
        }
        public IActionResult Index()
        {
            //ProjectInfo model = (ProjectInfo)_ProjectInfoContext.ProjectInfo.Where(m => m.Project_ID.Equals(projectID));
            //return View(model);
            return View();
        }
        public IActionResult RiskAssess(string projectID)
        {
            ViewBag.projectID = projectID;
            //價格
            var ConCost = _ProjectInfoContext.ProjectInfo
                                    .Where(x => x.Project_ID == projectID)
                                    .Select(x => x.ConstructionCost)
                                    .FirstOrDefault();
            bool ConstructionCostBool = int.TryParse(ConCost, out int ConstructionCost);
            ViewBag.ConstructionCost = ConstructionCost;

            var raList = _ProjectTasksRiskAssessContext.ProjectTasks.Where(m => m.Project_ID.Equals(projectID) && m.Task1Step != null && m.Task2Step != null)
                           .GroupJoin(_ProjectTasksRiskAssessContext.RiskAssess, pt => pt.id, risk => risk.TaskID, (pt, risk) => new { projectTask = pt, riskAssess = risk })
                            .SelectMany(combination => combination.riskAssess.DefaultIfEmpty(), (pt, risk) =>
                            new
                            {
                                pt.projectTask.id,
                                pt.projectTask.Task1Step,
                                pt.projectTask.Task2Step,
                                pt.projectTask.TaskContent,
                                pt.projectTask.SubProjectID,
                                pt.projectTask.SubProjectName,
                                risk.idx,
                                risk.PotentialHazard,
                                risk.PossibleSituation,
                                risk.RA_Possibility,
                                risk.RA_Level

                            }).OrderBy(m => m.SubProjectID).ToList();
            List<RiskAssessViewModel> RAModelList = new List<RiskAssessViewModel>();
            foreach (var item in raList)
            {
                RiskAssessViewModel raModel = new RiskAssessViewModel();
                raModel.idx = item.idx;
                raModel.TaskID = item.id;
                raModel.Task1Step = item.Task1Step;
                raModel.Task2Step = item.Task2Step;
                raModel.TaskContent = item.TaskContent;
                raModel.SubProjectName = item.SubProjectName;
                raModel.PotentialHazard = item.PotentialHazard;
                raModel.SubProjectID = item.SubProjectID;
                RAModelList.Add(raModel);
            }

            return View(RAModelList);

        }
        public IActionResult SubProjects(string projectID, string activeSubProject)
        {
            List<sys_SubProject>? sysSubPJList = _sysSubProjectContext?.sys_SubProject.ToList();
            ViewBag.sysSubProjectList = sysSubPJList;
            ViewBag.sysFirstSetpTask = "";
            ViewBag.projectID = projectID;
            ViewBag.activeSubProject = activeSubProject;
            List<ProjectTasks>? ptList = _ProjectTasksContext?.ProjectTasks.Where(m => m.Project_ID.Equals(projectID)).OrderBy(m => m.SubProjectID).ToList();

            return View(ptList);
        }

        public JsonResult SetSubProject(string subProjectID, string subProjectName, string projrctID, string firstStepTask, string secondStepTask, string taskContext, string stage)
        {
            var t = new ProjectTasks
            {
                Project_ID = projrctID,
                SubProjectID = subProjectID,
                SubProjectName = subProjectName,
                Task1Step = firstStepTask,
                Task2Step = secondStepTask,
                TaskContent = taskContext,
                Stage = stage
            };
            _ProjectTasksContext.Add(t);
            var r = _ProjectTasksContext.SaveChanges();
            var relult = getTasksInfo(projrctID, "", subProjectName);
            return Json(relult.Value);
        }
        public JsonResult EditTaskContect(string projectID, string subProjectName, string id, string taskContext)
        {
            var d = _ProjectTasksContext.ProjectTasks.Find(Int32.Parse(id));
            d.TaskContent = taskContext;
            _ProjectTasksContext.SaveChanges();
            var relult = getTasksInfo(projectID, "", subProjectName);
            return Json(relult.Value);

        }
        public JsonResult deleteTask(string projectID, string subProjectName, string taskId)
        {
            var d = _ProjectTasksContext.ProjectTasks.Find(Int32.Parse(taskId));

            _ProjectTasksContext.ProjectTasks.Remove(d);
            _ProjectTasksContext.SaveChanges();
            var relult = getTasksInfo(projectID, "", subProjectName);
            return Json(relult.Value);

        }
        public JsonResult get2StepSysTasks(string subProjectID)
        {
            var s = subProjectID.Split(',');
            var pjID = s[0];
            var task1stepId = s[1].Trim();

            var sys2ndSetpTask = _sysTask2StepContext?.Sys_Task2Step.Where(m => m.SubProjectID.Trim().Equals(pjID) & m.Task1StepID.Trim().Equals(task1stepId)).ToList();

            return Json(sys2ndSetpTask);
        }

        public JsonResult get1StepSysTasks(string subProjectID)
        {
            var s = subProjectID.Split(',');
            var sysFirstSetpTask = _sysTask1StepContext?.Sys_Task1Step.Where(m => m.SubProjectID.Equals(s[0])).ToList();
            return Json(sysFirstSetpTask);
        }
        public JsonResult getRiskAssessInfo(string projectID, string subProjectId, string subProjectName)
        {

            var raList = _ProjectTasksRiskAssessContext.ProjectTasks.Where(m => m.Project_ID.Equals(projectID) && m.Task1Step != null && m.Task2Step != null && m.SubProjectName.Equals(subProjectName))
                           .GroupJoin(_ProjectTasksRiskAssessContext.RiskAssess, pt => pt.id, risk => risk.TaskID, (pt, risk) => new { projectTask = pt, riskAssess = risk })
                            .SelectMany(combination => combination.riskAssess.DefaultIfEmpty(), (pt, risk) =>
                            new
                            {
                                pt.projectTask.id,
                                pt.projectTask.Task1Step,
                                pt.projectTask.Task2Step,
                                pt.projectTask.TaskContent,
                                pt.projectTask.SubProjectID,
                                pt.projectTask.SubProjectName,
                                risk.idx,
                                risk.PotentialHazard,
                                risk.PossibleSituation,
                                risk.RA_Possibility,
                                risk.RA_Level,
                                risk.TaskID,
                                risk.RiskCM,
                                risk.RA_Severity,
                                risk.CM_Persion,

                            }).OrderBy(m => m.SubProjectID).ToList();
            List<RiskAssessViewModel> riskAssessLists = new List<RiskAssessViewModel>();
            foreach (var item in raList)
            {
                RiskAssessViewModel raModel = new RiskAssessViewModel();
                raModel.idx = item.idx;
                raModel.TaskID = item.id;
                raModel.Task1Step = item.Task1Step;
                raModel.Task2Step = item.Task2Step;
                raModel.TaskContent = item.TaskContent;
                raModel.SubProjectName = item.SubProjectName;
                raModel.PotentialHazard = item.PotentialHazard;
                raModel.PossibleSituation = item.PossibleSituation;
                raModel.RA_Possibility = item.RA_Possibility;
                raModel.RA_Severity = item.RA_Severity;
                raModel.SubProjectID = item.SubProjectID;
                raModel.RiskCM = item.RiskCM;
                raModel.CMPersion = item.CM_Persion;
                riskAssessLists.Add(raModel);
            }
            return Json(riskAssessLists);
        }
        public JsonResult getTasksInfo(string projectID, string subProjectId, string subProjectName)
        {
            var currentProjectTasks = _ProjectTasksContext?.ProjectTasks.Where(m => m.SubProjectName.Equals(subProjectName) && m.Project_ID.Trim().Equals(projectID.Trim())).OrderBy(m => m.SubProjectID).ToList();
            List<ProjectTasks> TaskLists = new List<ProjectTasks>();
            var model = _ProjectTasksContext?.ProjectTasks.Where(m => m.SubProjectName.Equals(subProjectName) && m.Project_ID.Trim().Equals(projectID.Trim())).OrderBy(m => m.SubProjectID).GroupBy(x => new { x.SubProjectName, x.Task1Step }).Select(g => new { name = g.Key, count = g.Count() });
            foreach (var item in model)
            {
                foreach (var task in currentProjectTasks)
                {
                    if (item.name.Task1Step != null)
                    {
                        if (task.Task1Step != null)
                        {
                            if (item.name.Task1Step.Trim().Equals(task.Task1Step.Trim()) && task.Task2Step != null)
                            {
                                TaskLists.Add(task);
                            }
                            else if (item.name.Task1Step.Trim().Equals(task.Task1Step.Trim()) && item.count == 1)
                            {
                                TaskLists.Add(task);
                            }
                        }

                    }
                    else
                    {
                        TaskLists.Add(task);
                        break;
                    }
                }
            }

            return Json(TaskLists);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult verify()
        {
            return View();
        }
        RiskAssess? TaskRiskAssessId(int idx)
        {
            var ra = _RiskAssessContext.RiskAssess.Where(m => m.idx.Equals(idx)).FirstOrDefault();
            return ra;
        }
        public JsonResult SetRiskAssessInfo(int idx, int taskId, string potential_hazard, string possible_situation, int severity, int possibility, string riskCM, string modifiedFlag)
        {
            var result = new { Success = "True", Message = "Error Message" };
            var tra = TaskRiskAssessId(idx);
            if ((tra == null))
            {
                var t = new RiskAssess
                {
                    TaskID = taskId,
                    PotentialHazard = potential_hazard,
                    PossibleSituation = possible_situation,
                    CM_Persion = "張三豐",
                    CreatedDate = DateTime.Now
                };
                _RiskAssessContext?.Add(t);

            }
            else
            {
                tra.PotentialHazard = potential_hazard;
                tra.PossibleSituation = possible_situation;
                tra.RA_Severity = severity;
                tra.RA_Possibility = possibility;
                tra.RiskCM = riskCM;

            }
            _RiskAssessContext?.SaveChanges();
            return Json(result);
        }
        public JsonResult SetProjectInfo(string projectName, string stage)
        {
            var result = new { Success = "False", Message = "Error Message" };
            var pjInf = new ProjectInfo
            {
                Project_ID = DateTime.Now.ToString("yyMMddhhmmss"),
                UserID = HttpContext.Session.GetString("UserID"),
                Project_Name = projectName,
                Stage = stage,
                CreatedDate = DateTime.Now,
            };
            _ProjectInfoContext?.Add(pjInf);
            _ProjectInfoContext?.SaveChanges();
            return Json(result);
        }
        public IActionResult ProjectInfo(string projectID)
        {
            if (projectID == null)
            {
                return View();
            }
            else
            {
                return View(getProjectInfoViewModel(projectID));
            }
        }
        ProjectInfoViewModel getProjectInfoViewModel(string projectID)
        {
            ProjectInfo? pj = _ProjectInfoContext.ProjectInfo.Where(m => m.Project_ID.Equals(projectID)).FirstOrDefault();
            ProjectInfoViewModel pModel = new ProjectInfoViewModel();
            pModel.Project_ID = pj.Project_ID;
            pModel.Project_Name = pj.Project_Name;
            pModel.Location = pj.Location;
            pModel.Organizer_Principal = pj.Organizer_Principal;
            pModel.Organizer_PhoneNo = pj.Organizer_PhoneNo;
            pModel.Organizer_Addr = pj.Organizer_Addr;
            pModel.Organizer_Email = pj.Organizer_Email;
            pModel.Construction_Principal = pj.Construction_Principal;
            pModel.Construction_PhoneNo = pj.Construction_PhoneNo;
            pModel.Construction_Addr = pj.Construction_Addr;
            pModel.Construction_Email = pj.Construction_Email;
            pModel.PCM_Principal = pj.PCM_Principal;
            pModel.PCM_PhoneNo = pj.PCM_PhoneNo;
            pModel.PCM_Addr = pj.PCM_Addr;
            pModel.PCM_Email = pj.PCM_Email;
            pModel.Supervision_Principal = pj.Supervision_Principal;
            pModel.Supervision_PhoneNo = pj.Supervision_PhoneNo;
            pModel.Supervision_Addr = pj.Supervision_Addr;
            pModel.Supervision_Email = pj.Supervision_Email;
            pModel.Designer_Principal = pj.Designer_Principal;
            pModel.Designer_Addr = pj.Designer_Addr;
            pModel.Designer_PhoneNo = pj.Designer_PhoneNo;
            pModel.Designer_Email = pj.Designer_Email;
            pModel.Structure = pj.Structure;
            pModel.ConstructionCost = pj.ConstructionCost;
            pModel.UseFor = pj.UseFor;
            pModel.Notes = pj.Notes;
            return pModel;
        }
        [HttpPost]
        public IActionResult ProjectInfo(IFormCollection post)
        {
            ProjectInfo? pj = _ProjectInfoContext.ProjectInfo.Where(m => m.Project_ID.Equals(post["Project_ID"])).FirstOrDefault();
            if (pj != null)
            {
                pj.Location = post["Location"];
                pj.Organizer_Principal = post["Organizer_Principal"];
                pj.Organizer_Addr = post["Organizer_Addr"];
                pj.Organizer_PhoneNo = post["Organizer_PhoneNo"];
                pj.Organizer_Email = post["Organizer_Email"];
                pj.PCM_Principal = post["PCM_Principal"];
                pj.PCM_Addr = post["PCM_Addr"];
                pj.PCM_PhoneNo = post["PCM_PhoneNo"];
                pj.PCM_Email = post["PCM_Email"];
                pj.Designer_Principal = post["Designer_Principal"];
                pj.Designer_Addr = post["Designer_Addr"];
                pj.Designer_PhoneNo = post["Designer_PhoneNo"];
                pj.Designer_Email = post["Designer_Email"];
                pj.Supervision_Addr = post["Supervision_Addr"];
                pj.Supervision_PhoneNo = post["Supervision_PhoneNo"];
                pj.Supervision_Email = post["Supervision_Email"];
                pj.Supervision_Principal = post["Supervision_Principal"];
                pj.Construction_Addr = post["Construction_Addr"];
                pj.Construction_PhoneNo = post["Construction_PhoneNo"];
                pj.Construction_Email = post["Construction_Email"];
                pj.Construction_Principal = post["Construction_Principal"];
                pj.Structure = post["Structure"];
                pj.ConstructionCost = post["ConstructionCost"];
                pj.UseFor = post["UseFor"];
                pj.Notes = post["Notes"];

            }
            _ProjectInfoContext?.Update(pj);
            _ProjectInfoContext?.SaveChanges();
            return View(getProjectInfoViewModel(post["Project_ID"].ToString()));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection post)
        {
            string account = post["account"];
            string password = post["password"];
            var user = _UserContext.User.Where(m => m.Account.Trim().Equals(account)).FirstOrDefault();
            if (user != null && user.Password.Trim().Equals(password))
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                //return View("ProjectList");
                return Redirect("ProjectList?userID=" + user.UserID.ToString());//參數可為空的跳轉方法
            }
            else
                ViewBag.LoginMsg = "帳號或密碼錯誤!";


            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Register(RegisterVierwModel RegisterData)
        {
            //判斷帳號有沒有一樣
            var data = _UserContext.User.Where(x => x.Account == RegisterData.Account).FirstOrDefault();
            try
            {
                if (data == null)
                {
                    string verifyPassword = randomNumber();//獲取驗證碼
                    //發送信件
                    SendEmail(RegisterData.Email, $"<p>您的驗證碼為{verifyPassword}<p>");
                    var dbUuser = new User
                    {
                        Password = RegisterData.Password,
                        Account = RegisterData.Account,
                        Email = RegisterData.Email,
                        ValidateCode = verifyPassword,
                        UserID = "202210",
                        Name = "eddy",
                    };
                    _UserContext.User.Add(dbUuser);
                    _UserContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return Json(new { data = "失敗" });
            }

            return Json(new { data = "成功" });
        }
        [HttpPost]
        public IActionResult verify(string ValidateCode, string account)
        {
            var user = _UserContext.User.Where(x => x.Account == account).FirstOrDefault();
            if (user != null && user.ValidateCode == ValidateCode)
            {
                user.ValidateCode = "";
                _UserContext.SaveChanges();
                return Json(new { data = "成功" });
            }
            return Json(new { data = "失敗" });
        }
        public IActionResult ProjectList(string userID)
        {
            List<ProjectInfo>? ls = _ProjectInfoContext?.ProjectInfo.Where(m => m.UserID.Trim().Equals(userID)).ToList();
            return View(ls);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //public PartialViewResult _ProjectTasks(ProjectTasks model)
        //{
        //    return PartialView(model); // returns view with model
        //}

        /// <summary>
        /// 發送信箱
        /// </summary>
        /// <param name="userEmail">要發送的信箱</param>
        /// <param name="mailContent">要發送的內容</param>
        /// <returns>返回發送信箱的結果</returns>
        public static bool SendEmail(string userEmail, string mailContent)
        {
            //發送信件
            var message = new MimeMessage();
            // 添加寄件者
            message.From.Add(new MailboxAddress("註冊通知", "eddytw1999@gmail.com"));

            // 添加收件者
            message.To.Add(new MailboxAddress("New Member", $"{userEmail}"));
            // 設定郵件標題
            message.Subject = "註冊";

            // 使用 BodyBuilder 建立郵件內容
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = mailContent;

            // 設定郵件內容
            message.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("eddytw1999@gmail.com", "auqsqzqhtbyphtsk");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// 生成驗證碼
        /// </summary>
        /// <returns></returns>
        public static string randomNumber()
        {
            string vc = "";
            Random rNum = new Random();
            int num1 = rNum.Next(0, 9);
            int num2 = rNum.Next(0, 9);
            int num3 = rNum.Next(0, 9);
            int num4 = rNum.Next(0, 9);

            int[] nums = new int[4] { num1, num2, num3, num4 };
            for (int i = 0; i < nums.Length; i++)
            {
                vc += nums[i].ToString();
            }
            return vc;
        }
    }
}