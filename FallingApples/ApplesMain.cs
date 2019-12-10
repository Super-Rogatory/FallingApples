/********************************************************
 * Author: Chukwudi Ikem
 * Email: godofcollege43@csu.fullerton.edu
 * Course: CPSC 223N
 * Semester: Fall 2019
 * Assignment #: 5
 * Program name: FallingApples
*****************************************************/

using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace FallingApples
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the Falling Apples!");
            Application.Run(new AppleUI());

        }
    }
}
