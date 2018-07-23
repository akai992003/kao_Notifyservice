using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotifyService.Data;
using NotifyService.Models;

namespace NotifyService.Controllers {
    public class ItemController : Controller {
        private IhnNotifyItemService _IhnNotifyItemService;
        private IhnNotifyService _IhnNotifyService;
        private IMemberService _IMemberService;

        public ItemController (IhnNotifyItemService IhnNotifyItemService,
            IhnNotifyService IhnNotifyService,
            IMemberService IMemberService) {
            this._IhnNotifyItemService = IhnNotifyItemService;
            this._IhnNotifyService = IhnNotifyService;
            this._IMemberService = IMemberService;
        }
        public async Task<IActionResult> Index () {
            var q = await this._IhnNotifyItemService.ShowItem ();
            return View (q);
        }

        public async Task<IActionResult> ShowMember (int membercounter) {
            var q = await this._IhnNotifyService.ShowMemberItem (membercounter);

            DTOhnNotifyItemUser _dto = new DTOhnNotifyItemUser ();
            _dto.member_counter = membercounter;
            _dto.DTOhnNotifyItems = q;

            return View (_dto);
        }

        [HttpPost]
        public IActionResult ShowMember (DTOhnNotifyItemUser dto) {

            this._IhnNotifyService.UpdateNotifyMember (dto);

            return RedirectToAction ("ServiceList", "Item", new { membercounter = dto.member_counter});
        }

        public IActionResult Login (string result = "") {
            //若要帶入預設值到頁面，可用@viewbag或@viewdata
            // 例如@viewbag.text1="akai";
            //    @viewdata["text2"]="hello2";
            if (result != "") {
                ViewBag.result = result;
            } else {
                ViewBag.result = "";
            }

            return View ();
        }
        // public IActionResult fail () {

        //     return View ();
        // } 
        // public IActionResult welcom (string name) {
        //     ViewData["name"] = name;
        //     return View ();
        // }
         public IActionResult ServiceList (int membercounter) {
            int _membercounter = membercounter;
           
            var q = this._IhnNotifyService.ShowMemberOrderItem(_membercounter);
            ViewBag.member_counter = _membercounter;
            return View (q);
         }

        [HttpPost]
        public async Task<IActionResult> Login (Members member) {
            var q = member.account;
            var p = member.password;

            // 登入的SERVICE回傳使用者counter  
            var m = await _IMemberService.accountLogin (q, p);

            // 無此帳號
            if (m.counter == -1) {
                // string _result ="沒有這個帳號" ;

                return RedirectToAction ("login", "Item", new { result = "沒有這個帳號" });
                //return RedirectToAction ("fail", "Item", null);
            } else if (m.counter == 0) {
                //    登入失敗

                //string _result = m.name + " ,登入失敗" ;
                //return RedirectToAction ("error", "Item", new { name = m.name });
                // fail = views的名稱Item=controller前面的名稱，大小寫要一樣
                //每個controller在views裡面都會有個自已的資料夾
                //該資料夾裡面的CSHTEM檔是對應IActionResult(也就是Action))
                return RedirectToAction ("login", "Item", new { result = m.name + " ,登入失敗" });
            } else {

                // return RedirectToAction ("welcom", "Item", new { name = m.name });
                return RedirectToAction ("ShowMember", "Item", new { membercounter = m.counter});
            }

        }

        public IActionResult DeleteNotify(int member_counter,string serviceName,string actionName){
                return View ();
        }
    }
}