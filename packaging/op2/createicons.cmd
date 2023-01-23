set IMG_MAGICK_DIR="C:\Program Files\ImageMagick-7.0.11-Q16\magick.exe"

%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -define icon:auto-resize mod.ico
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 16x16 icon_16x16.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 24x24 icon_24x24.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 32x32 icon_32x32.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 48x48 icon_48x48.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 64x64 icon_64x64.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 96x96 icon_96x96.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 128x128 icon_128x128.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 256x256 icon_256x256.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 512x512 icon_512x512.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 1024x1024 icon_1024x1024.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 1280x640 github_socialmedia_1280x640.png
%IMG_MAGICK_DIR% convert -transparent white -background None -density 1024 %cd%\openop2.svg -resize 252x252 banner_252x252.png
pause