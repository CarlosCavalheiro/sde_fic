using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    
    public class HomeController : Controller
    {
        private DBConnect _db = new DBConnect();
     
        
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Message = "Acesso restrito!";
            return View();
        }

        //
        // GET: /Home/        
        public ActionResult Sobre()
        {
            ViewBag.Message = "Explicação Sobre o Software";
            return View();
        }

        //
        // GET: /Home/
        public ActionResult Contato()
        {
            ViewBag.Message = "Forma de contato sem logar.";
            return View();
        }

        //
        // GET: /Home/
        public ActionResult Login()
        {
            ViewBag.Message = "Digite seu usuário, senha e clique em acessar!";        
            return View();
        }

        //
        // POST: /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario u)
        {            

            UsuarioDAO usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();

            _usuario = usuarioDAO.LoadByUsername(u.Username);

            if (_usuario == null)
            {
                ViewBag.msg_Error = "Nome de USUÁRIO não está correto!"; 
                ViewBag.Message = "Tente novamente ou entre em contato."; 
            }
            else
            {
                if (_usuario.Password != u.Password)
                {
                    ViewBag.msg_Error = "A SENHA não está correta!";
                    ViewBag.Message = "Tente novamente ou entre em contato com Coordenador.";
                }
                else 
                { 
                    Session["LogedUserID"] = _usuario.IdUsuario;
                    Session["LogedUserName"] = _usuario.Username;
                    Session["LogedUserPerfil"] = _usuario.Perfil;
                    Session["LogedUserPassword"] = _usuario.Password;
                    Session["LogedUserNomeCompleto"] = _usuario.NomeCompleto;

                    return RedirectToAction("Index", "Sistema");
                }
                
            }

            return View("Index", u);
          
        }

        public ActionResult AfterLogin()
        {
            if (Session["LogedUserID"] != null)
            {
                return View("Index", "Home");
            }
            else            
            {
                return RedirectToAction("Index","Home");
            }
            
        }
  
        public ActionResult Logout()
        {
            if (Session["LogedUserID"] != null)
            {
                Session["LogedUserID"] = null;
                ViewBag.Message = "Realizado Logout pelo usuário.";
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ViewBag.Message = "Usuário não está logado.";
                return View("Index", "Home");
            }
            
        }  

    }
}
