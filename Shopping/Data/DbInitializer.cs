using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Authorization;
using Shopping.Models;

namespace Shopping.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "rfoskett@shopping.com");
                await EnsureRole(serviceProvider, adminID, Constants.UserAdministratorsRole);

                var adminID2 = await EnsureUser(serviceProvider, testUserPw, "mmughal@shopping.com");
                await EnsureRole(serviceProvider, adminID2, Constants.UserAdministratorsRole);

                var adminID3 = await EnsureUser(serviceProvider, testUserPw, "jsun@shopping.com");
                await EnsureRole(serviceProvider, adminID3, Constants.UserAdministratorsRole);

                SeedDB(context, adminID, adminID2, adminID3);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string adminID, string adminID2, string adminID3)
        {

            if (!context.categories.Any())
            {
                // Seeding for category.
                var categories = new Category[]
                {
                new Category
                {
                    Name = "Computers, Tablets & Accessories"
                },
                new Category
                {
                    Name = "Cell Phones and Accessories"
                },
                new Category {
                    Name = "TV & Home Theatre"
                },
                new Category
                {
                    Name = "Headphones, Speakers & Audio"
                },
                new Category
                {
                    Name = "Cameras & Camcorders"
                }
                };
                foreach (Category category in categories)
                {
                    context.categories.Add(category);
                }
                context.SaveChanges();
            }

            // Seeding for product.
            if (!context.products.Any() && context.categories.Any())
            {
                var products = new Product[]
                {
                    new Product
                    {
                        Name = "Apple MacBook Air 13.6\" w/ Touch ID (2022) - Midnight (Apple M2 Chip / 256GB SSD / 8GB RAM) - English",
                        Description = "Apple’s thinnest and lightest notebook gets supercharged with the Apple M2 chip. The M2 chip starts the next generation of Apple silicon, with even more of the speed and power efficiency of M1. So you can get more done faster with a more powerful 8‑core CPU. Create captivating images and animations with up to a 8-core GPU and work with more streams of 4K and 8K ProRes video.",
                        Price = 1500.44,
                        Image = "/MacBookAir.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "ASUS VivoBook S 15.6\" OLED Laptop - Indie Black (Intel Evo i5-12500H /1TB SSD/16GB RAM/Windows 11)",
                        Description = "The ASUS Vivobook S15 laptop is Intel EVO verified which means you'll have fast charge, all-day battery, blazing fast internet speeds, instant wake, as well as incredible performance from its Intel Core i5-12500H CPU. The OLED display offers ultra vivid colours with a 100% DCI-P3 colour gamut and is Pantone validated for the highest colour accuracy and deepest blacks for every image.",
                        Price = 1100.44,
                        Image = "/AsusVivoBook.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "HP 15.6\" Laptop - Natural Silver (Intel Core i5-1235U/1TB SSD/16GB RAM/Windows 11)",
                        Description = "Bring efficiency to your home office with this 15.6-inch HP laptop. With its Intel Core i5-1235U processor with 1.3GHz processing speed and 16GB of RAM, you'll speed through important work and personal tasks. The generous 1TB storage drive lets you store all of your necessary files, and the 720p webcam and microphone allow you to take calls with crystal clear audio and video.",
                        Price = 1000.44,
                        Image = "/HPLaptop.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "HP OMEN 25L Gaming PC (AMD Ryzen 7 5700G/512GB SSD/16GB RAM/RTX 3070)",
                        Description = "Delve into your favourite gaming world with the HP Omen 25L gaming PC. Boasting a dedicated NVIDIA GeForce RTX 3070 graphics card of 8GB GDDR6 video memory, this computer churns out realistic visuals with high frame rates for all the latest AAA titles. Plus, its AMD Ryzen 7 5700G processor and 16GB RAM deliver ample power to process multiple workflows at once.",
                        Price = 1800.64,
                        Image = "/HPOMEN.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "ASUS ROG Strix G35CG Gaming PC (Intel Core i9-11900KF/1TB SSD/16GB RAM/RTX 3080/Windows 11)",
                        Description = "Play your favourite games at home or with friends with the ASUS ROG Strix G35CG Gaming PC. It features a transparent side panel and a built-in handle for easy transport and is powered by the Intel Core i9-11900KF processor and 16GB RAM for incredible performance. The NVIDIA GeForce RTX 3080 graphics card and Intel Z590 Chipset sound card ensure incredible graphics and rich sound for an immersive gaming experience.",
                        Price = 3000.64,
                        Image = "/ASUSROGjpg.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "Apple iPad Pro 12.9\" 128GB with Wi-Fi (5th Generation) - Space Grey",
                        Description = "Unlock productivity and entertainment on the go with the Apple iPad Pro. Powered by the Apple M1 chip, it's equipped for lightning fast performance so you can focus on what matters. It boasts 2 cameras and a Liquid Retina XDR display that delivers vivid colours and stunning detail, so you can edit HDR photos and videos. Pair it with the Apple Pencil, Magic Keyboard with trackpad, or Smart Keyboard Folio (sold separately) for the ultimate experience.",
                        Price = 1400.44,
                        Image = "/iPadPro.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Tab S8 Ultra 14.6\" 256GB Android 11 Tablet w/ Qualcomm SM8450 8-Core Processor - Graphite",
                        Description = "Live an ultimate lifestyle with the Samsung Galaxy Tab S8 Ultra tablet. It's powered with the Qualcomm 8540 8-core processor, 12GB RAM, and 256GB storage capacity. Featuring a 14.6\" sAMOLED screen, ultra-wide dual front camera with auto framing, and ultra-fast S Pen, this premium tablet gives you more space to work and create professional-quality projects, with enhanced multitasking on the go.",
                        Price = 1300.44,
                        Image = "/SamsungGalaxyTab.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "HP 15.6\" Laptop - Natural Silver (Intel Core i5-1235U/1TB SSD/16GB RAM/Windows 11)",
                        Description = "Bring efficiency to your home office with this 15.6-inch HP laptop. With its Intel Core i5-1235U processor with 1.3GHz processing speed and 16GB of RAM, you'll speed through important work and personal tasks. The generous 1TB storage drive lets you store all of your necessary files, and the 720p webcam and microphone allow you to take calls with crystal clear audio and video.",
                        Price = 1000.44,
                        Image = "/HPLaptop.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "Amazon Fire HD 8 8\" 32GB FireOS Tablet with MTK/MT8168 4-Core Processor - Black",
                        Description = "Slim and lightweight, the Amazon Fire HD 8 tablet is a great tool for all your daily needs. With a 2.0GHz quad-core MTK/MT8168 processor and 2GB of RAM, this powerful tablet has a 32GB hard drive, dual-band Wi-Fi, and access to a world of apps, games, and more. The gorgeous 8\" touchscreen display and all-day battery life mean you'll be set no matter where you are.",
                        Price = 80.44,
                        Image = "/AmazonFire.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "LG UltraWide 34\" FHD 100Hz 5ms GTG LED IPS FreeSync Gaming Monitor (34WQ650-W) - White",
                        Description = "Enjoy competitive gaming like never before with the LG UltraWide 34\" FHD gaming monitor. This monitor integrates an IPS display with AMD FreeSync technology to deliver seamless gaming at 100Hz refresh rate and 5ms response time. It supports the VESA DisplayHDR 400 to offer life-like visuals and the ultrawide aspect ratio of 21:9 assures incredible viewing without any hidden scenes.",
                        Price = 335.99,
                        Image = "/LGUltraWide.jpg",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Z Fold4 5G 512GB - Greygreen - Unlocked",
                        Description = "Enjoy innovative flexibility anywhere with the Samsung Galaxy Z Fold4 5G smartphone. This unique device unfolds into a tablet and enables hands-free viewing so you can effortlessly go from pocket to table. Complete multiple tasks at once with the Multi-Window feature, and play games and stream videos seamlessly with its vibrant display and sturdy, water-resistant build.",
                        Price = 2269.99,
                        Image = "/SamsungGalaxyZFold.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Z Flip4 5G 256GB - Blue - Unlocked",
                        Description = "Small but powerful the Galaxy Z Flip4 fits everything you need in one compact-sized smartphone. It features an ultra thin foldable full screen display that allows you to do more in one screen. Catch up with friends on the top screen while find a post-worthy selfie on the bottom. Plus, it boasts the Flexcam feature that lets you take selfies hands-free so you're sure everybody's in the frame.",
                        Price = 1259.99,
                        Image = "/Samsung2.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Google Pixel 6a 128GB - Charcoal - Unlocked",
                        Description = "The Google Pixel 6a is fast and adaptable phone. Built with Google Tensor, the powerful chip designed by Google just for Pixel, this phone is smart, secure, and powerful. Launch apps and load images or pages quickly as well as take beautiful shots with intelligent photography that helps you shoot and enhance images. It boasts an incredible performance that's made for all you are.",
                        Price = 549.99,
                        Image = "/Google1.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Apple iPhone 13 Pro 128GB - Alpine Green - Unlocked",
                        Description = "Explore endless possibilities with the iPhone 13 Pro. It features the powerful A15 Bionic chip, superfast 5G to download and stream high-quality video, a bright 6.1\" Super Retina XDR display with ProMotion, and Ceramic Shield for better drop performance. Other features include Pro camera system with new 12MP Telephoto, Wide and Ultra Wide cameras, extra-ordinary battery life, and much more.",
                        Price = 1399.99,
                        Image = "/16001802.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Google Pixel 6 128GB - Stormy Black - Unlocked",
                        Description = "Stay in the moment with modern and intuitive Google Pixel 6 5G phone. With a powerful camera system, next-gen Titan M2 security, and the custom-built Google Tensor processor, it's the smartest and fastest Pixel yet. With faster apps and pages, an all-day battery and proactive help, it delivers what you need when you need it.",
                        Price = 699.99,
                        Image = "/15748206.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Apple iPhone XR 64GB Smartphone - Black - Unlocked - Open Box",
                        Description = "READ SELLER'S STORE DESCRIPTION FOR MORE INFO. Open Box: 10/10 condition product with manufacturer or seller warranty, only difference with Factory Fresh is open packaging; original accessories included except headphones. Some products sold may be international versions of this device and will be fully compatible with Canada’s mobile networks and seller-provided warranty.",
                        Price = 351.99,
                        Image = "/13527658.jpeg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Apple iPhone 13 128GB - Pink - Unlocked",
                        Description = "Stand out and show-off your superpowers with the iPhone 13. It features the powerful A15 Bionic chip, superfast 5G to download and stream high-quality video, a bright 6.1\" Super Retina XDR display, and Ceramic Shield for better drop performance. Other features include 4K Dolby Vision HDR recording, an advanced dual-camera system with Night mode, extra-ordinary battery life, and much more.",
                        Price = 1099.99,
                        Image = "/15723465.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Apple iPhone SE 128GB (3rd Generation) - Midnight - Unlocked",
                        Description = "Enjoy big-time performance that lasts long with the Apple iPhone SE (3rd Generation). Built with the powerful A15 Bionic chip and a battery that delivers up to 15 hours of video playback, this iPhone brings speed to everything you do. It features a 4.7-inch display, a Home button with secure Touch ID, and a 12MP wide superstar camera that makes smart adjustments so you get picture perfect shots.",
                        Price = 649.99,
                        Image = "/16001796.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S20 FE 128GB Smartphone - Cloud Lavender - Unlocked - Open Box",
                        Description = "READ SELLER'S STORE DESCRIPTION FOR MORE INFO. Open Box: Unused, 10/10 condition product with manufacturer or seller warranty, only difference with Factory Fresh is open packaging; original accessories included except headphones. Some products sold may be international versions of this device and will be fully compatible with Canada’s mobile networks and seller-provided warranty.",
                        Price = 435.99,
                        Image = "/15222475.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S22 Ultra 5G 128GB - Green - Unlocked",
                        Description = "Make everyday moments come to life with the innovative Samsung Galaxy S22 Ultra 5G smartphone. With 108MP photo resolution and 8K video, this mobile phone is built to capture memories that's important to you. It features night mode for your crystal-clear nightography, 48-hour battery for unstoppable action, and the embedded S Pen to boost your creativity.",
                        Price = 1379.99,
                        Image = "/15952621.jpg",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "TCL 6-Series 75\" 4K UHD HDR QLED Roku OS Smart TV (75R635-CA)",
                        Description = "Stay entertained with all your movies, TV shows, and games in cinematic quality with this TCL 6-Series 75\" smart TV. Featuring QLED, Ultra HD resolution, HDR Pro Pack, Natural Motion 480, and a variety of gaming technologies, this television delivers impressive visuals with sharp imagery. It works on the intuitive Roku TV platform so you can stream a world of content.",
                        Price = 2013.99,
                        Image = "/14671251.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "LG 65\" 4K UHD HDR OLED webOS Smart TV (OLED65C1AUB) - 2021",
                        Description = "Bring the cinematic experience right to your home with the LG 65\" HDR smart TV. Boasting a self-lit OLED display, it presents your movies and other content in Ultra HD 4K resolution with realistic colours and precise image details. Additionally, it features webOS operating system that is intuitive, and gives you access to an array of entertainment apps to stream movies, shows, and more.",
                        Price = 2013.99,
                        Image = "/15442034.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "TCL 4-Series 75\" 4K UHD LED Direct Lit Roku OS Smart TV (75S45-CA) - 2022",
                        Description = "Stream content in stunning 4K Ultra HD with the TCL 4-Series Roku Smart TV. This television offers a 75\" screen and comes preloaded with a variety of popular streaming apps, like Netflix and Prime Video, as well as dozens of live TV channels for endless entertainment. With its intuitive home screen and remote, it's easy to find something to watch.",
                        Price = 813.99,
                        Image = "/16309925.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Samsung 70\" 4K UHD HDR LED Tizen Smart TV (UN70TU7000FXZC) - Titan Grey",
                        Description = "Enjoy long hours of entertainment on this Samsung 70\" 4K UHD LED smart TV that provides realistic pictures in everything from sports to movies. Its HDR and PurColor technology enhance the display with natural colours, while the Crystal Processor 4K turns up the clarity. Its Wi-Fi connectivity helps in streaming your favourite online content.",
                        Price = 1013.99,
                        Image = "/14469354.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Samsung 65\" 4K UHD HDR QLED Tizen Smart TV (QN65Q60BAFXZC) - Titan Grey",
                        Description = "Enjoy crystal clear picture quality with this Samsung 65-inch Smart TV. The 4K UHD display is enhanced by QLED technology, allowing you to play games, watch movies, and more with lag-free performance and vivid colours. It also offers surround sound audio enhancements for a more immersive listening experience and wireless connection capabilities.",
                        Price = 1213.99,
                        Image = "/16009746.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "LG 65\" 4K UHD HDR OLED webOS Evo ThinQ AI Smart TV (OLED65C2PUA) - 2022 - Dark Titan Silver",
                        Description = "Create the entertainment space you always dreamt of with the LG 65\" 4K OLED Evo ThinQ AI smart TV. Powered by the a9 Gen 5 AI Processor 4K, this smart TV features a 65\" panel with slim bezel for spectacular images with perfect blacks, wide viewing angles, and impressive motion performance. Designed for cinematic viewing as well as gaming, it gives you a complete entertainment experience.",
                        Price = 2613.99,
                        Image = "/16157495.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Sony X80K 65\" 4K UHD HDR LED Smart Google TV (KD65X80K) - 2022",
                        Description = "Bring big screen entertainment to your home with this Sony 65\" 4K UHD HDR LED smart Google TV. With impressive sound and picture processing capabilities, this TV delivers 4K HDR content and lets you access movies, videos, shows and apps with Google TV in exceptional clarity and detail. Preloaded with YouTube, Netflix and Amazon Prime video, this TV offers instant and endless entertainment.",
                        Price = 1113.99,
                        Image = "/15966921.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Samsung The Frame 65\" 4K UHD HDR QLED Tizen OS Smart TV (QN65LS03AAFXZC) - 2021",
                        Description = "Invite gallery-inspired beauty into your home with this 65\" Samsung The Frame smart TV. It features a stunning 4K Ultra HD display with advanced technologies so you can enjoy a cinematic movie experience or display art from the Art Store in living colour. The built-in Tizen operating system ensures seamless, lag-free performance.",
                        Price = 1813.99,
                        Image = "/15337617.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Samsung The Frame 50\" 4K UHD HDR QLED Tizen Smart TV (QN50LS03BAFXZC) - 2022 - Charcoal Black",
                        Description = "Create an immersive and artistic home theatre experience with the 50-inch Samsung The Frame smart TV in your living room. It comes preloaded with popular apps, allowing you to immediately start enjoying your favourite movies and television shows in clear, vivid 4K Ultra HD. It also transforms into a customizable work of art for ambiance.",
                        Price = 1413.99,
                        Image = "/16032840.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Toshiba 50\" 4K UHD HDR LED Smart TV (50C350KC) - Fire TV Edition - 2021 - Only at Best Buy",
                        Description = "Bring your home entertainment to life with this 50\" Toshiba smart TV. It delivers your movies, shows, and games in vibrant 4K Ultra HD resolution so you can immerse yourself in the action. It uses the Fire TV operating system to give you instant access to an incredible variety of streaming channels while integrated Amazon Alexa makes operation effortless.",
                        Price = 583.99,
                        Image = "/15465117.jpg",
                        CategoryId = 3
                    },
                    new Product
                    {
                        Name = "Sony MDR-ZX110 Over-Ear Headphones - Black",
                        Description = "Enjoy powerful sound from a compact, lightweight design with these foldable headphones from Sony. Featuring a stylish on-ear design, these headphones are the perfect accessory to take to the streets.",
                        Price = 25.49,
                        Image = "/10325154.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Beats by Dr. Dre Studio3 Skyline Over-Ear Noise Cancelling Bluetooth Headphones - Midnight Black",
                        Description = "Enjoy a premium listening experience and complete wireless freedom with the Beats Studio3 over-ear headphones. Pure Adaptive Noise Canceling (Pure ANC) actively blocks unwanted external noise, while real-time audio calibration preserves the clarity, range, and emotion in your favourite music.",
                        Price = 250.49,
                        Image = "/13188016.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "JBL Tune 500BT On-Ear Bluetooth Headphones - Black",
                        Description = "Transform your listening experience with these JBL Tune 500BT on-ear headphones. Bluetooth technology lets you pair them effortlessly with your device while powerful drivers with JBL Pure Bass Sound deliver incredible audio quality. Their lightweight design and padded headband ensure enhanced comfort for long listening sessions.",
                        Price = 35.49,
                        Image = "/15458144.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Klipsch R41M 50-Watt Bookshelf Speaker - Pair - Black",
                        Description = "Powerful sound and versatile functionality come in a small package with the Klipsch Reference R-41M monitor speaker. This speaker delivers theatre-worthy sound in a compact size, so it can perform on its own or as a left, centre, right, or surround speaker without taking up too much of your valuable space.",
                        Price = 281.99,
                        Image = "/12656543.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Samsung SWA-9100S/ZC Wireless Rear Speaker Kit - Pair",
                        Description = "Get room-filling sound with this Samsung 9100 wireless rear speaker kit. It works with select 2021 Samsung soundbar modesl and adds a surround-sound experience to your entertainment centre.",
                        Price = 201.99,
                        Image = "/15321495.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Samsung HW-S61B 200-Watt 5.0 Channel Sound Bar - White",
                        Description = "Enjoy crystal clear, room-filling sound with the Samsung HW-S61B sound bar. This audio accessory delivers 200 watts of power and is equipped with 5.0 channels for dynamic bass and audible dialogue throughout your living space. Connect via Wi-Fi, Bluetooth, or Airplay 2 to stream music, movies, and more wirelessly.",
                        Price = 501.99,
                        Image = "/16000040.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Bose TV Speaker Bluetooth Soundbar",
                        Description = "Turn every movie, TV show, and video game into a cinematic experience with the Bose Solo Bluetooth TV speaker. Boasting a compact size that fits virtually anywhere, this sound bar fills your room with rich, powerful sound and enhances the clarity of dialogue so you catch every spoken word. It connects to your TV in a snap so you'll be up and running in no time.",
                        Price = 301.99,
                        Image = "/14801063.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Sonos One (2nd Gen) Voice Controlled Smart Speaker w/ Amazon Alexa and Google Assistant - Black",
                        Description = "This second generation of Sonos One boasts increased memory and an updated processor. It also blends great sound with seemless voice assistance. Simply ask for a song from services like Spotify, Amazon Music, iHeartRadio, and TuneIn. Then, you can enjoy the rich, room-filling audio while you lounge and relax. Pair with other Sonos speakers for whole-home enjoyment.",
                        Price = 271.99,
                        Image = "/13446573.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Bose Home Speaker 500 Wireless Multi-Room Speaker with Voice Control Built-In - Triple Black",
                        Description = "The Bose Home Speaker 500 delivers wall-to-wall stereo sound from a single speaker. Built-in voice control from Google Assistant and Amazon Alexa puts songs, playlists, and more at the tip of your tongue. And you have the freedom to control the music your way — with your voice, with a tap on the top controls, or with the app.",
                        Price = 481.99,
                        Image = "/12907057.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Sonos Five Wireless Multi-Room Speaker - Single - Black",
                        Description = "Control the music you play anywhere in your home with the Sonos Five wireless multi-room speaker. Tuned by the famous producer Giles Martin, this Wi-Fi-enabled speaker delivers a wide soundscape with deep, thumping bass. Play music or podcasts by connecting it to Apple AirPlay 2 or other streaming devices.",
                        Price = 701.99,
                        Image = "/14591240.jpg",
                        CategoryId = 4
                    },
                    new Product
                    {
                        Name = "Canon PowerShot G7 X Mark II Wi-Fi 20.1MP 4.2x Optical Zoom Digital Camera - Black",
                        Description = "Spark creativity with the press of a shutter button. The Canon PowerShot G7 X Mark II delivers powerful performance with its 20.1MP CMOS image sensor and DIGIC 7 processor, capturing stunning, detail images. It has a 3-inch, 180-degree swiveling LCD touchscreen display perfect for selfies and other creative shots. It also has built-in Wi-Fi and NFC.",
                        Price = 800.49,
                        Image = "/10414355.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Canon PowerShot ELPH 360 HS WiFi 20.2MP 12x Optical Zoom Digital Camera - Black",
                        Description = "Your everyday life is filled with magic moments. Capture them all with the Canon PowerShot ELPH 360 HS. It features a 20.2MP CMOS sensor and DIGIC 4+ image processor that captures still up to 7.2fps and Full HD video. It has 12x optical zoom that gets you closer to your subject and built-in WiFi and NFC ensures easy sharing with your mobile device.",
                        Price = 300.49,
                        Image = "/10406706.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "DJI Action 2 Camera Power Combo",
                        Description = "Capture stunning photos and videos with the DJI Action 2 camera power combo. Its lightweight and portable design makes it your perfect companion to record all those unforgettable moments. A 4K/120fps resolution allows you to shoot videos in sharp, lifelike detail. It also includes an adapter mount and two magnetic mounts to shoot from any angle for more creative video footages.",
                        Price = 360.49,
                        Image = "/15777881.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Insta360 ONE X2 Waterproof 360 Degree Action Camera, 5.7K, Touch Screen, AI Editing, Live Streaming, Webcam, Voice Control",
                        Description = "The Pocket Camera Crew: With just one device, capture every angle at once with super 5.7K 360 capture and H.265 encoding. Or pick just one lens with Steady Cam mode for ultra-stable, wide angle footage.",
                        Price = 637.86,
                        Image = "/15317861.jpeg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "GoPro MAX Waterproof 5.6K Sport & Helmet Camera with Compact Case",
                        Description = "Relive every action-packed moment with the GoPro MAX waterproof 5.6K sport and helmet camera. It has all the capabilities of the traditional HERO camera while providing additional features like 360-degree footage and instant panoramic shots. It's ruggedly designed for any adventure and comes with a compact case for traveling convenience.",
                        Price = 670.49,
                        Image = "/15371494.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Sony RX0 II 15.3MP Waterproof Advanced Compact Digital Camera",
                        Description = "Effortlessly capture amazing photos and video footage on all your adventures with the Sony RX0 II advanced digital camera. This ultra-compact, ruggedly crafted camera is water, shock, and crush-proof, so it can go anywhere you go. It's loaded with advanced technologies and features to deliver exceptional shots with ease and convenience.",
                        Price = 899.47,
                        Image = "/13497598.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Fujifilm Instax Mini Evo Instant Camera - Black",
                        Description = "Capture moments, print, and share them promptly with the Fujifilm Instax Mini Evo instant camera. It includes 10 integrated lenses and film effects that allow you to capture amazing moments and scenes. The sleek design lets you activate printing by pulling the lever, play with the dial to adjust the film, or turn the lens to switch between different effects.",
                        Price = 250.49,
                        Image = "/15951930.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Fujifilm Instax Mini 11 Instant Camera Bundle - Sky Blue - Only at Best Buy",
                        Description = "Capture and print memories with the Fujifilm INSTAX Mini 11 instant camera. Featuring auto exposure, you can snap the best shots in any lighting condition. One touch selfie mode extends the lens so you can take selfies without having to attach a separate lens. This bundle also comes with 10 mini film exposures, a photo album, and photo clips.",
                        Price = 130.49,
                        Image = "/14964967.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Canon EOS Rebel T7 DSLR Camera with 18-55mm Lens Kit",
                        Description = "Enjoy new perspectives with the Canon offers the EOS Rebel T7 DSLR camera. From everyday pictures of your kids and pets to stunningly beautiful nature photographs, this camera and lens kit can easily handle all photographs. The full range of features makes this a perfect camera choice.",
                        Price = 600.49,
                        Image = "/12383478.jpg",
                        CategoryId = 5
                    },
                    new Product
                    {
                        Name = "Sony Alpha a7 II Full-Frame Mirrorless Camera with FE 28-70mm Lens Kit",
                        Description = "Capture images as they were meant to be seen with the Sony a7 II mirrorless camera. Its 24.3MP full-frame Exmor sensor and BIONZ X processor faithfully reproduce colours, textures, and details as seen by the naked eye. Built-in 5-axis image stabilization compensates for 5 different types of camera shake. Includes a FE 28-70mm f/3.5-5.6 OSS lens.",
                        Price = 2000.49,
                        Image = "/10347557.jpg",
                        CategoryId = 5
                    }

            };
                foreach (Product product in products)
                {
                    context.products.Add(product);
                }
                context.SaveChanges();
            }

            // Seeding for Admin-Users
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            context.User.AddRange(
                new User
                {
                    Name = "Reilly Foskett",
                    Email = "rfoskett@shopping.com",
                    AdminID = adminID,
                    Password = "password"
                },
                new User
                {
                    Name = "Moosa Mughal",
                    Email = "mmughal@shopping.com",
                    AdminID = adminID2,
                    Password = "password"
                },
                new User
                {
                    Name = "Jingwen Sun",
                    Email = "jsun@shopping.com",
                    AdminID = adminID3,
                    Password = "password"
                }
            );
            context.SaveChanges();
        }
    }
}
