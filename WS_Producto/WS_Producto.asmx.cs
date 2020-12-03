using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ClaseDatos;
using System.Web.Security;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WS_Producto
{
    [WebService(Namespace = "http://cesion.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_Producto : System.Web.Services.WebService
    {

        [WebMethod(Description = "Solicitar Acceso")]
        public ClaseEntradaSalida.MENSAJERES_Type SolicitarAcceso(ClaseEntradaSalida.CABECERA_Type cabecera, ClaseEntradaSalida.DATOS_Type datosWC)

        {
            /****** DECLARAR TIPO DE DATOS PARA RESPUESTA********/
            ClaseEntradaSalida.MENSAJERES_Type MENSAJERES = new ClaseEntradaSalida.MENSAJERES_Type();
            ClaseEntradaSalida.INTEGRACIONRES_Type INTEGRES = new ClaseEntradaSalida.INTEGRACIONRES_Type();
            ClaseEntradaSalida.CABECERARes_Type CABECERA = new ClaseEntradaSalida.CABECERARes_Type();
            ClaseEntradaSalida.DETALLERes_Type DETALLE = new ClaseEntradaSalida.DETALLERes_Type();
            ClaseEntradaSalida.DATOSRes_Type DATOS = new ClaseEntradaSalida.DATOSRes_Type();
            
            /***RESPUESTA PARA LA EJECUCION ERRONEA***/
            string passwordDesencriptado = "";
            CABECERA.APP_CONSUMIDORA = cabecera.APP_CONSUMIDORA;
            CABECERA.COD_RESPUESTA = "1";
            CABECERA.DES_RESPUESTA = "Ejecución Erronea";
            DATOS.resultCode = null;
            DATOS.message = null;
            DETALLE.DATOS = DATOS;
            INTEGRES.CABECERA = CABECERA;
            INTEGRES.DETALLE = DETALLE;
            MENSAJERES.INTEGRES = INTEGRES;

            /****DESENCRIPTAMOS EL PASSWORD RECIBIDO (QUEDA COMO TEXTO PLANO)***/
            passwordDesencriptado = Desencriptar(datosWC.Key,datosWC.Password);


            //SI EL PASSWORD DESENCRIPTADO ES DISTINTO DE "" SIGNIFICA QUE SE DESENCRIPTO BIEN
            if (passwordDesencriptado!="")
            {    
            try {
                    /***UNA VEZ OBTENIDO EL PASSWORD COMO TEXTOPLANO***/
                    /***ESTE SE DEBE CODIFICAR AL  SISTEMA DE ENCRIPTACION USADO POR EL CLIENTE***/
                    /***Y FINALMENTE COMPARARLO CON EL DE LA BD***/
                   
                    
                    //SOLO A MODO DE EJEMPLO USAREMOS "123456"
                    if (datosWC.Usuario == "isobarzo@dim.cl" && passwordDesencriptado == "123456")

                    {
                    //SI EL EMAIL Y EL PASSWORD CONCUERDAN SE ENVIA RESPUESTA CORRECTA
                    CABECERA.APP_CONSUMIDORA = cabecera.APP_CONSUMIDORA;
                    CABECERA.COD_RESPUESTA = "1";
                    CABECERA.DES_RESPUESTA = "Ejecución Correcta";
                    DATOS.resultCode = "1";
                    DATOS.message="Usuario Valido";
                    DETALLE.DATOS = DATOS;
                    INTEGRES.CABECERA = CABECERA;
                    INTEGRES.DETALLE = DETALLE;
                    MENSAJERES.INTEGRES = INTEGRES;
                }
                else
                {
                    //SI EL EMAIL Y EL PASSWORD SON DIFERENTES SE ENVIA RESPUESTA ERRONEA
                    CABECERA.APP_CONSUMIDORA = cabecera.APP_CONSUMIDORA;
                    CABECERA.COD_RESPUESTA = "1";
                    CABECERA.DES_RESPUESTA = "Ejecución Correcta";
                    DATOS.resultCode = "0";
                    DATOS.message = "Usuario No Valido";
                    DETALLE.DATOS = DATOS;
                    INTEGRES.CABECERA = CABECERA;
                    INTEGRES.DETALLE = DETALLE;
                    MENSAJERES.INTEGRES = INTEGRES;
                }


            }
            catch (Exception ex)
            {
                return MENSAJERES;
            }
            }
            else
            {
                return MENSAJERES;

            }
            return MENSAJERES;
        }




        /**DESENCRIPTA LA PASSWORD PASANDOLE LA SEMILLA O KEY RECIBIDA**/
        public static string Desencriptar(string key, string palabraEncriptada)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(palabraEncriptada);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return "";
            }
        }


    }
}
