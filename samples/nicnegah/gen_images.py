"""Draw the NicNegah placeholder images (services, samples, hero background, favicon, share image) into uploads/.
These stand in for real photos: replace the files with photos of the same name later."""
import math, os, random, sys
from PIL import Image, ImageDraw, ImageFilter

HERE = os.path.dirname(os.path.abspath(__file__))
OUT = os.path.join(HERE, "uploads")
SS = 2  # supersample

CYAN, TEAL, INK = (0, 255, 234), (0, 168, 160), (11, 37, 48)
MAG, YEL, PAPER, WHITE = (230, 0, 126), (255, 212, 0), (246, 244, 239), (255, 255, 255)
SOFT = {"cyan": (214, 247, 244), "mag": (251, 226, 239), "yel": (255, 246, 207), "paper": (238, 236, 230)}


def canvas(w, h, bg):
    return Image.new("RGB", (w * SS, h * SS), bg)


def save(im, name, w, h):
    im = im.resize((w, h), Image.LANCZOS)
    im.save(os.path.join(OUT, "nicnegah-" + name), quality=88, optimize=True)


def shadowed(im, draw_fn, offset=(10, 16), blur=18):
    """Draw a soft shadow of a shape, then the shape."""
    sh = Image.new("L", im.size, 0)
    draw_fn(ImageDraw.Draw(sh), True, offset)
    sh = sh.filter(ImageFilter.GaussianBlur(blur * SS))
    im.paste((0, 0, 0), (0, 0), sh.point(lambda v: int(v * 0.18)))
    draw_fn(ImageDraw.Draw(im), False, (0, 0))


def rot_rect(cx, cy, w, h, ang):
    a = math.radians(ang)
    pts = [(-w / 2, -h / 2), (w / 2, -h / 2), (w / 2, h / 2), (-w / 2, h / 2)]
    return [(cx + x * math.cos(a) - y * math.sin(a), cy + x * math.sin(a) + y * math.cos(a)) for x, y in pts]


def business_cards():
    W, H = 800, 600
    im = canvas(W, H, SOFT["cyan"])
    for i, (ang, col, dx, dy) in enumerate([(-14, INK, -70, 30), (-4, WHITE, 0, 0), (8, TEAL, 70, -40)]):
        cx, cy, w, h = (400 + dx) * SS, (300 + dy) * SS, 340 * SS, 200 * SS
        def f(d, mask, off, cx=cx, cy=cy, w=w, h=h, ang=ang, col=col):
            pts = [(x + off[0] * SS, y + off[1] * SS) for x, y in rot_rect(cx, cy, w, h, ang)]
            d.polygon(pts, fill=255 if mask else col)
        shadowed(im, f)
    d = ImageDraw.Draw(im)
    # details on the top (teal) card
    cx, cy = 470 * SS, 260 * SS
    a = math.radians(8)
    def p(x, y):
        return (cx + x * SS * math.cos(a) - y * SS * math.sin(a), cy + x * SS * math.sin(a) + y * SS * math.cos(a))
    r = 26 * SS
    c = p(110, -45)
    d.ellipse([c[0] - r, c[1] - r, c[0] + r, c[1] + r], fill=CYAN)
    for k, ln in enumerate([170, 120, 90]):
        d.line([p(-140, 20 + k * 22), p(-140 + ln, 20 + k * 22)], fill=WHITE, width=7 * SS)
    save(im, "service-business-cards.jpg", W, H)


def brochure():
    W, H = 800, 600
    im = canvas(W, H, SOFT["mag"])
    panels = [[(180, 140), (340, 170), (340, 470), (180, 440)], [(340, 170), (480, 140), (480, 440), (340, 470)],
              [(480, 140), (630, 170), (630, 470), (480, 440)]]
    cols = [WHITE, (250, 250, 250), WHITE]
    for pts, col in zip(panels, cols):
        def f(d, mask, off, pts=pts, col=col):
            d.polygon([((x + off[0]) * SS, (y + off[1]) * SS) for x, y in pts], fill=255 if mask else col)
        shadowed(im, f, blur=14)
    d = ImageDraw.Draw(im)
    d.polygon([(x * SS, y * SS) for x, y in [(180, 140), (340, 170), (340, 300), (180, 270)]], fill=MAG)
    d.ellipse([520 * SS, 200 * SS, 590 * SS, 270 * SS], fill=YEL)
    for k in range(5):
        y = 320 + k * 24
        d.line([(200 * SS, y * SS), (320 * SS, (y + 18) * SS)], fill=(200, 200, 205), width=6 * SS)
        d.line([(360 * SS, (y - 40) * SS), (460 * SS, (y - 62) * SS)], fill=(200, 200, 205), width=6 * SS)
        d.line([(500 * SS, (y - 10) * SS), (610 * SS, (y + 12) * SS)], fill=(200, 200, 205), width=6 * SS)
    save(im, "service-brochure.jpg", W, H)


def banner():
    W, H = 800, 600
    im = canvas(W, H, SOFT["yel"])
    def f(d, mask, off):
        d.rounded_rectangle([(300 + off[0]) * SS, (70 + off[1]) * SS, (500 + off[0]) * SS, (500 + off[1]) * SS], 6 * SS,
                            fill=255 if mask else INK)
    shadowed(im, f)
    d = ImageDraw.Draw(im)
    d.rounded_rectangle([270 * SS, 498 * SS, 530 * SS, 530 * SS], 14 * SS, fill=(120, 120, 128))
    d.ellipse([340 * SS, 120 * SS, 460 * SS, 240 * SS], fill=CYAN)
    d.ellipse([370 * SS, 150 * SS, 430 * SS, 210 * SS], fill=INK)
    for k, ln in enumerate([140, 110, 140, 80]):
        d.line([(330 * SS, (290 + k * 30) * SS), ((330 + ln) * SS, (290 + k * 30) * SS)], fill=WHITE, width=9 * SS)
    d.rectangle([300 * SS, 440 * SS, 500 * SS, 500 * SS], fill=YEL)
    save(im, "service-banner.jpg", W, H)


def labels():
    W, H = 800, 600
    im = canvas(W, H, SOFT["paper"])
    def f(d, mask, off):
        d.rounded_rectangle([(170 + off[0]) * SS, (80 + off[1]) * SS, (630 + off[0]) * SS, (520 + off[1]) * SS], 18 * SS,
                            fill=255 if mask else WHITE)
    shadowed(im, f)
    d = ImageDraw.Draw(im)
    cols = [CYAN, MAG, YEL, INK, TEAL, MAG, YEL, CYAN, INK]
    for i in range(3):
        for j in range(3):
            cx, cy = 260 + j * 140, 170 + i * 130
            c = cols[i * 3 + j]
            if (i + j) % 2 == 0:
                d.ellipse([(cx - 50) * SS, (cy - 50) * SS, (cx + 50) * SS, (cy + 50) * SS], fill=c)
                d.ellipse([(cx - 22) * SS, (cy - 22) * SS, (cx + 22) * SS, (cy + 22) * SS], fill=WHITE)
            else:
                d.rounded_rectangle([(cx - 55) * SS, (cy - 38) * SS, (cx + 55) * SS, (cy + 38) * SS], 14 * SS, fill=c)
                d.line([((cx - 30) * SS, cy * SS), ((cx + 30) * SS, cy * SS)], fill=WHITE, width=7 * SS)
    save(im, "service-labels.jpg", W, H)


def packaging():
    W, H = 800, 600
    im = canvas(W, H, SOFT["cyan"])
    top = [(400, 110), (590, 200), (400, 290), (210, 200)]
    left = [(210, 200), (400, 290), (400, 520), (210, 430)]
    right = [(400, 290), (590, 200), (590, 430), (400, 520)]
    def f(d, mask, off):
        d.polygon([((x + off[0]) * SS, (y + off[1]) * SS) for x, y in top + right[2:] + left[2:]], fill=255 if mask else WHITE)
    shadowed(im, f)
    d = ImageDraw.Draw(im)
    d.polygon([(x * SS, y * SS) for x, y in top], fill=(230, 252, 250))
    d.polygon([(x * SS, y * SS) for x, y in left], fill=TEAL)
    d.polygon([(x * SS, y * SS) for x, y in right], fill=INK)
    d.line([(305 * SS, 155 * SS), (495 * SS, 245 * SS)], fill=(190, 225, 222), width=10 * SS)
    d.ellipse([455 * SS, 330 * SS, 535 * SS, 410 * SS], fill=CYAN)
    d.line([(240 * SS, 330 * SS), (370 * SS, 392 * SS)], fill=WHITE, width=8 * SS)
    save(im, "service-packaging.jpg", W, H)


def invitations():
    W, H = 800, 600
    im = canvas(W, H, SOFT["mag"])
    def env(d, mask, off):
        d.rectangle([(180 + off[0]) * SS, (230 + off[1]) * SS, (560 + off[0]) * SS, (480 + off[1]) * SS], fill=255 if mask else (240, 228, 214))
    shadowed(im, env)
    d = ImageDraw.Draw(im)
    d.polygon([(180 * SS, 230 * SS), (370 * SS, 360 * SS), (560 * SS, 230 * SS)], fill=(226, 210, 192))
    def card(d2, mask, off):
        pts = [(x + off[0] * SS, y + off[1] * SS) for x, y in rot_rect(500 * SS, 260 * SS, 230 * SS, 320 * SS, 9)]
        d2.polygon(pts, fill=255 if mask else WHITE)
    shadowed(im, card)
    d = ImageDraw.Draw(im)
    c = (500 * SS, 200 * SS)
    d.ellipse([c[0] - 40 * SS, c[1] - 40 * SS, c[0] + 40 * SS, c[1] + 40 * SS], outline=MAG, width=6 * SS)
    for k, ln in enumerate([120, 150, 90]):
        y = 280 + k * 26
        d.line([((500 - ln / 2) * SS, y * SS), ((500 + ln / 2) * SS, y * SS)], fill=(205, 205, 210), width=6 * SS)
    save(im, "service-invitations.jpg", W, H)


def swatches():
    W, H = 1200, 800
    im = canvas(W, H, (233, 238, 238))
    cols = [INK, (0, 120, 140), TEAL, CYAN, (120, 230, 120), YEL, (255, 140, 0), MAG, (140, 0, 120)]
    pivot = (600 * SS, 690 * SS)
    for i, c in enumerate(cols):
        ang = -60 + i * 15
        def f(d, mask, off, ang=ang, c=c):
            a = math.radians(ang)
            L, Wd = 520 * SS, 92 * SS
            pts = []
            for x, y in [(-Wd / 2, 0), (Wd / 2, 0), (Wd / 2, -L), (-Wd / 2, -L)]:
                pts.append((pivot[0] + x * math.cos(a) - y * math.sin(a) + off[0] * SS,
                            pivot[1] + x * math.sin(a) + y * math.cos(a) + off[1] * SS))
            d.polygon(pts, fill=255 if mask else c)
        shadowed(im, f, offset=(4, 6), blur=8)
    d = ImageDraw.Draw(im)
    d.ellipse([pivot[0] - 22 * SS, pivot[1] - 22 * SS, pivot[0] + 22 * SS, pivot[1] + 22 * SS], fill=(200, 200, 205))
    save(im, "sample-swatches.jpg", W, H)


def sheets():
    W, H = 1200, 800
    im = canvas(W, H, INK)
    d = ImageDraw.Draw(im)
    for k in range(4):
        def f(d2, mask, off, k=k):
            pts = [(x + off[0] * SS, y + off[1] * SS) for x, y in rot_rect((600 - k * 14) * SS, (400 + k * 10) * SS, 620 * SS, 440 * SS, -6 + k * 3)]
            d2.polygon(pts, fill=255 if mask else (250, 250, 248))
        shadowed(im, f, blur=12)
    d = ImageDraw.Draw(im)
    # crop marks and colour bar on the top sheet
    x0, y0, x1, y1 = 310, 200, 890, 600
    for (x, y, dx, dy) in [(x0, y0, -1, -1), (x1, y0, 1, -1), (x0, y1, -1, 1), (x1, y1, 1, 1)]:
        d.line([((x + dx * 10) * SS, y * SS), ((x + dx * 40) * SS, y * SS)], fill=INK, width=3 * SS)
        d.line([(x * SS, (y + dy * 10) * SS), (x * SS, (y + dy * 40) * SS)], fill=INK, width=3 * SS)
    for i, c in enumerate([CYAN, MAG, YEL, INK]):
        d.rectangle([(360 + i * 60) * SS, 540 * SS, (410 + i * 60) * SS, 570 * SS], fill=c)
    d.ellipse([560 * SS, 250 * SS, 820 * SS, 510 * SS], fill=CYAN)
    d.ellipse([640 * SS, 330 * SS, 740 * SS, 430 * SS], fill=INK)
    for k, ln in enumerate([170, 140, 170, 110]):
        d.line([(370 * SS, (270 + k * 34) * SS), ((370 + ln) * SS, (270 + k * 34) * SS)], fill=(170, 175, 180), width=10 * SS)
    save(im, "sample-sheets.jpg", W, H)


def halftone(name, W, H, bg, colors, max_r, step):
    im = canvas(W, H, bg)
    d = ImageDraw.Draw(im)
    for ci, (col, cx, cy, spread) in enumerate(colors):
        for y in range(0, H + step, step):
            for x in range(0, W + step, step):
                xx, yy = x + (step / 2 if (y // step) % 2 else 0) + ci * 3, y + ci * 3
                dist = math.hypot(xx - cx, yy - cy) / spread
                r = max_r * max(0.0, 1 - dist)
                if r > 0.6:
                    d.ellipse([(xx - r) * SS, (yy - r) * SS, (xx + r) * SS, (yy + r) * SS], fill=col)
    save(im, name, W, H)


def favicon():
    """Logo on a dark rounded square, so it reads on light and dark browser tabs."""
    N = 512
    im = Image.new("RGBA", (N, N), (0, 0, 0, 0))
    ImageDraw.Draw(im).rounded_rectangle([0, 0, N - 1, N - 1], 110, fill=(8, 25, 31, 255))
    logo = Image.open(os.path.join(OUT, "nicnegah-logo.png")).convert("RGBA")
    logo.thumbnail((int(N * 0.62), int(N * 0.78)), Image.LANCZOS)
    im.alpha_composite(logo, ((N - logo.width) // 2, (N - logo.height) // 2))
    im.resize((192, 192), Image.LANCZOS).save(os.path.join(OUT, "nicnegah-favicon.png"), optimize=True)


def og_image(bold_ttf, regular_ttf):
    """1200x630 share image: halftone dark background, logo, Persian name and tagline (needs Vazirmatn TTFs)."""
    from PIL import ImageFont
    W, H = 1200, 630
    halftone("og-bg.tmp.jpg", W, H, (8, 25, 31), [(TEAL, 1100, 120, 650), ((120, 0, 70), 80, 640, 420)], 7, 20)
    path = os.path.join(OUT, "nicnegah-og-bg.tmp.jpg")
    im = Image.open(path).convert("RGBA")
    os.remove(path)
    logo = Image.open(os.path.join(OUT, "nicnegah-logo.png")).convert("RGBA")
    logo.thumbnail((300, 380), Image.LANCZOS)
    im.alpha_composite(logo, (110, (H - logo.height) // 2))
    d = ImageDraw.Draw(im)
    big = ImageFont.truetype(bold_ttf, 120, layout_engine=ImageFont.Layout.RAQM)
    small = ImageFont.truetype(regular_ttf, 38, layout_engine=ImageFont.Layout.RAQM)
    d.text((1090, 230), "نیک نگاه", font=big, fill=(255, 255, 255), anchor="rs", direction="rtl", language="fa")
    d.text((1090, 330), "چاپ دیجیتال", font=small, fill=CYAN, anchor="rs", direction="rtl", language="fa")
    d.text((1090, 410), "کارت ویزیت، بروشور، بنر، لیبل و بسته‌بندی", font=small, fill=(190, 210, 214), anchor="rs",
           direction="rtl", language="fa")
    im.convert("RGB").save(os.path.join(OUT, "nicnegah-og-image.png"), optimize=True)


if __name__ == "__main__":
    os.makedirs(OUT, exist_ok=True)
    random.seed(4)
    business_cards(); brochure(); banner(); labels(); packaging(); invitations()
    swatches(); sheets()
    halftone("sample-halftone.jpg", 1200, 800, PAPER, [(CYAN, 380, 380, 520), (MAG, 860, 300, 420), (YEL, 700, 700, 380)], 11, 26)
    halftone("hero-bg.jpg", 1600, 900, INK, [(TEAL, 1250, 300, 750), ((120, 0, 70), 250, 820, 600)], 9, 24)
    favicon()
    if len(sys.argv) == 3:  # python3 gen_images.py Vazirmatn-ExtraBold.ttf Vazirmatn-Regular.ttf
        og_image(sys.argv[1], sys.argv[2])
    print("done")
