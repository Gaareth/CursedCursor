from PIL import Image
import tqdm
import time
import os
import Iconolatry
import numpy as np
import cv2
import imutils
import argparse


# Remove old images from output_path
def clear(dir_path):
    for f in os.listdir(dir_path):
        os.remove(os.path.join(dir_path, f))
    for f in os.listdir(dir_path+"_cur"):
        os.remove(os.path.join(dir_path+"_cur", f))
    print("Cleared!")


# Preferred method of rotating (I don't know why I chose this one)
def rotate_img_pil(img_path, rt_degr):
    img = Image.open(img_path)

    x = img.rotate(rt_degr, expand=1, resample=Image.BICUBIC)

    x = x.crop(box=(x.size[0] / 2 - img.size[0] / 2,
                    x.size[1] / 2 - img.size[1] / 2,
                    x.size[0] / 2 + img.size[0] / 2,
                    x.size[1] / 2 + img.size[1] / 2))

    #print(x.size)
    return x


def rotate_img_cv(image, angle):
    image = cv2.imread(image)
    image_center = tuple(np.array(image.shape[1::-1]) / 2)
    rot_mat = cv2.getRotationMatrix2D(image_center, angle, 1.0)
    result = cv2.warpAffine(image, rot_mat, image.shape[1::-1], flags=cv2.INTER_LINEAR, borderValue=(255,255,255),borderMode=cv2.BORDER_CONSTANT)
    return result


def rotate_img_imutils(image, angle):
    image = cv2.imread(image)
    rotated = imutils.rotate(image, angle)
    return rotated


def rot_cv(input, output):
    file_ext = os.path.splitext(input)[1]
    for angle in tqdm.tqdm(range(-180, 180)):
        cursor = rotate_img_cv(input, angle)
        cv2.imwrite(output+"\\default_cursor_" + str(angle) + file_ext, cursor)


def rotate_images(input, output):
    file_ext = os.path.splitext(input)[1]
    print("\n\n> Rotating")
    for angle in tqdm.tqdm(range(-180, 180)):
        cursor = rotate_img_pil(input, angle)
        cursor.save(output+"\\default_cursor_" + str(angle) + file_ext)


# Convert Image to cur format
def convert_to_cur(out):
    print("\n\n> Converting")
    for filename in tqdm.tqdm(os.listdir(out)):
        log_mess = Iconolatry.WRITER().ToIco(False, [[out+"\\"+filename]],[out+"_cur\\"+filename.split(".")[0]+".cur"])


if __name__ == "__main__":

    parser = argparse.ArgumentParser(description='Generate Cursor files')
    parser.add_argument('-i', "--input", type=str, required=True,
                        help='Image of base cursor')

    parser.add_argument('-o', "--output", type=str, required=True,
                        help='Output of the rotated images')


    args = parser.parse_args()

    input_path = args.input  # Image of base cursor
    output_path = args.output  # Output of the rotated images in .png format
    output_cursor_path = args.input+"_cur"  # Output of the rotated images in .cur format

    if not os.path.exists(output_path):
        os.mkdir(output_path)

    if not os.path.exists(output_cursor_path):
        os.mkdir(output_cursor_path)

    clear(output_path)

    rotate_images(input=input_path, output=output_path)
    convert_to_cur(output_path)


    print("\nCursor icons created")
