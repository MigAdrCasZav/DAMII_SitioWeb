using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAMII_SitioWeb.Models;

namespace DAMII_SitioWeb.Controllers
{
    public class RecetaController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Receta
        public ActionResult Index()
        {
            return View();
        }

        List<Receta> listRecetas()
        {
            List<Receta> aRecetas = new List<Receta>();
            SqlCommand cmd = new SqlCommand("SP_LISTARECETAS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Receta objR = new Receta()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    tipocomida = dr[2].ToString(),
                    video = dr[3].ToString(),
                    foto = dr[4].ToString(),
                    tiempoprepa = dr[5].ToString(),
                    cantidad = int.Parse(dr[6].ToString()),
                    ingredienes = dr[7].ToString(),
                    preparacion = dr[8].ToString(),
                    categoria = dr[9].ToString()
                };
                aRecetas.Add(objR);
            }
            dr.Close();
            cn.Close();
            return aRecetas;
        }

        List<Categoria> listCategorias()
        {
            List<Categoria> aCategorias = new List<Categoria>();
            SqlCommand cmd = new SqlCommand("SP_LISTACATEGORIA", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria objC = new Categoria()
                {
                    codcategoria = int.Parse(dr[0].ToString()),
                    descategoria = dr[1].ToString()
                };
                aCategorias.Add(objC);
            }
            cn.Close();
            return aCategorias;
        }

        public ActionResult listadoRecetas()
        {
            return View(listRecetas());
        }

        void CRUD(String proceso, List<SqlParameter> pars)
        {
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(proceso, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars.ToArray());
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
            }
            cn.Close();
        }

        public ActionResult nuevaReceta()
        {
            ViewBag.ultimo = listRecetas().Last().codigo+1;
            ViewBag.categoria = new SelectList(listCategorias(), "codcategoria", "descategoria");       
            return View(new RecetaO());
        }

        [HttpPost]
        public ActionResult nuevaReceta(RecetaO objR, HttpPostedFileBase f)
        {
            if (f == null)
            {
                ViewBag.mensaje = "Seleccione una imagen";
                return View(objR);
            }
            if (Path.GetExtension(f.FileName) != ".jpg")
            {
                ViewBag.mensaje = "Debe ser .JPG";
                return View(objR);
            }

            List<SqlParameter> lista = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName="@NOM",SqlDbType=SqlDbType.VarChar,Value=objR.nombre},
                new SqlParameter(){ParameterName="@TCOM",SqlDbType=SqlDbType.VarChar,Value=objR.tipocomida},
                new SqlParameter(){ParameterName="@VID",SqlDbType=SqlDbType.VarChar,Value=objR.video},
                new SqlParameter(){ParameterName="@FOT",SqlDbType=SqlDbType.VarChar,Value="~/fotos_recetas/"+Path.GetFileName(f.FileName)},
                new SqlParameter(){ParameterName="@TPRE",SqlDbType=SqlDbType.VarChar,Value=objR.preparacion},
                new SqlParameter(){ParameterName="@CANT",SqlDbType=SqlDbType.VarChar,Value=objR.cantidad},
                new SqlParameter(){ParameterName="@INGR",SqlDbType=SqlDbType.VarChar,Value=objR.ingredienes},
                new SqlParameter(){ParameterName="@PREP",SqlDbType=SqlDbType.VarChar,Value=objR.preparacion},
                new SqlParameter(){ParameterName="@CCAT",SqlDbType=SqlDbType.VarChar,Value=objR.categoria}
            };
            ViewBag.categoria = new SelectList(listCategorias(), "codcategoria", "descategoria");
            CRUD("SP_NUEVARECETA", lista);
            f.SaveAs(Path.Combine(Server.MapPath("~/fotos_recetas/"),Path.GetFileName(f.FileName)));
            return RedirectToAction("listadoRecetas");
        }

    }

}