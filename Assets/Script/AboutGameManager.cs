using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with detailed game info in Hindi.
/// </summary>
public class AboutGameManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TMP_Text component for the About section.")]
    public TMP_Text aboutText;

    void Start()
    {
        if (aboutText == null)
        {
            Debug.LogError("AboutGameManager: 'aboutText' reference is missing.");
            return;
        }

        aboutText.text =
            "<b>yksftdk ds ckjs esa</b>\n" +
            "yksftdk ,d Hkfo\";oknh y‚ftd ;q) [ksy gS tgk¡ nks ,vkbZ c‚V vkeus&lkeus vkrs gSa" +
            ",d gkMZdksMsM fu;eksa ¼çrhdkRed rdZ½ }kjk lapkfyr] vkSj nwljs yfuZax ,Yxksfjne ¼e'khu yfuZax½ }kjkA\n" +
            ",d ekuo f[kykM+h ds :i esa] vki bl eap esa mrjrs gSa vkSj nksuksa c‚V~l dks pqukSrh nsrs gSa] " +
            "viuh rdZ {kerk dh ijh{kk ysrs gq, ns[ksaxs fd ,vkbZ dSls lksprk gS] lh[krk gS] vkSj dHkh&dHkh xyfr;k¡ Hkh djrk gSA\n\n" +

            "<b>;g xse fdl ckjs esa gS\\</b>\n" +
            ",d opZqvy ySc ds Hkhrj lsV] yksftdk —f=e cqf)eÙkk ds lcls iqjkus ç'u dk irk yxkrk gS:\n\n" +
            "<i>D;k e'khusa lksp ldrh gSa\\</i>\n\n" +
            "vki y‚ftd pqukSfr;k¡ ysrs gSa tks fn[kkrh gSa fd çkjafHkd ,vkbZ ¼tSls rdZ fl)karoknh½ us fuf'pr fu;eksa ls " +
            "leL;k,¡ dSls gy dha] vkSj lkFk gh ;g Hkh ns[krs gSa fd vkèkqfud yfuZax&vkèkkfjr ,vkbZ mUgsa dSls vyx rjhds ls gy djrk gSA\n\n" +

            "<b>nks c‚V~l] nks ekufldrk,¡</b>\n" +
            "<b>:yc‚V</b> Dykfld ,vkbZ dh rjg lksprk gS foospukRed] çrhdkRed] lVhdA\n" +
            "<b>yuZc‚V</b> vkèkqfud ,e,y flLVe dh rjg lksprk gS iSVuZ&vkèkkfjr] çkf;fddh;] vuqdwyuh;A\n\n" +
            "dHkh os lger gksrs gSaA\n" +
            "dHkh os ,d&nwljs dk fojksèk djrs gSaA\n" +
            "dHkh&dHkh vki nksuksa ls Hkh rst+ fnekx gksaxsA\n\n" +

            "<b>vki D;k lh[ksaxs</b>\n" +
            "• çrhdkRed rdZ çkjafHkd ,vkbZ dh jh<+ dSls curk gS\n" +
            "• fu;e&vkèkkfjr ,vkbZ fdl {ks= esa mR—\"V gksrk gS — vkSj dgk¡ vlQy\n" +
            "• e'khu yfuZax vfuf'prrk dks dSls laHkkyrh gS\n" +
            "• vkèkqfud cqf)eku ç.kkfy;k¡ cukus ds fy, nksuksa dh le> D;ksa egRoiw.kZ gS\n\n" +

            "<b>[ksyus ds fy, rS;kj\\</b>\n" +
            "eap esa dne j[ksaA c‚V~l dks ekr nsaA\n" +
            "tkusa fd ,vkbZ dSls lh[krk gS vkSj igys dSls lksprk FkkA\n" +
            "yksftdk esa vkidk Lokxr gSA";
    }
}
