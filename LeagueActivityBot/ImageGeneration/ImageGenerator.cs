using System.Threading.Tasks;
using CoreHtmlToImage;

namespace LeagueActivityBot.ImageGeneration
{
    public static class ImageGenerator
    {
        public static byte[] Gen()
        {
            var converter = new HtmlConverter();
            
            var html = @"
                <html><head> <style>@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500&display=swap'); .container{width: 200px; max-width: 200px; font-family: 'Montserrat', sans-serif; font-weight: 400; border: 1px solid black;}.header{display: flex; flex-direction: row; align-items: center; justify-content: space-between; margin-bottom: 10px;}.avatar{width: 43px; height: 43px; border-radius: 50%;}.avatar-border{display: flex; justify-content: center; align-items: center; width: 45px; height: 45px; border-radius: 50%; background: linear-gradient(129.22deg, rgba(57, 168, 249, 0.88) 11.65%, #255C84 91.46%); margin: 10px 0 0 15px; box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.25);}.result{font-size: 24px; font-weight: 500; text-transform: uppercase; margin: 10px 20px 0 0;}.victory{color: rgba(255, 181, 38, 0.88);}.type{display: flex; flex-direction: column; align-items: flex-end;}.defeat{color: rgba(234, 62, 62, 0.88);}.game-type{font-size: 12px; margin: 0 20px 0 0;}.name{font-weight: 500; font-size: 24px; line-height: 29px; text-align: center; color: #000000; margin-bottom: 10px;}.main{display: flex; justify-content: center; margin-bottom: 13px;}.stats{display: flex; flex-direction: column; justify-content: space-between; margin-left: 10px;}.progress-bar{--b: 8px; --w: 60px; width: var(--w); aspect-ratio: 1; position: relative; display: inline-grid; place-content: center; font-size: 12px;}.progress-bar:before, .progress-bar:after{content: ''; position: absolute; border-radius: 50%;}.progress-bar:before{inset: 0; background: radial-gradient(farthest-side, var(--c) 98%, #0000) top/var(--b) var(--b) no-repeat, conic-gradient(var(--c) calc(var(--p)*1%), #0000 0); -webkit-mask: radial-gradient(farthest-side, #0000 calc(99% - var(--b)), #000 calc(100% - var(--b))); mask: radial-gradient(farthest-side, #0000 calc(99% - var(--b)), #000 calc(100% - var(--b)));}.progress-bar:after{inset: calc(50% - var(--b)/2); background: var(--c); transform: rotate(calc(var(--p)*3.6deg)) translateY(calc(50% - var(--w)/2));}.damage{font-size: 15px;}.score, .kda{font-size: 12px;}</style></head><body> <div class='container'> <header class='header'> <div class='avatar-border'> <img class='avatar' src='https://ddragon.leagueoflegends.com/cdn/12.9.1/img/champion/Caitlyn.png'> </div><div class='type'> <p class='result victory'>victory</p><p class='game-type'>ARAM</p></div></header> <div class='name'>DesireName</div><main class='main'> <div class='progress-bar' style='--p:87;--c:rgba(57, 168, 249, 1)'>87%</div><div class='stats'> <div class='damage'>50,203 DMG</div><div> <div class='score'>10/11/32</div><div class='kda'>3.8 KDA</div></div></div></main> </div></body></html>
            ";
            
            var bytes = converter.FromHtmlString(html,200);
            
            return bytes;
        } 
    }
}