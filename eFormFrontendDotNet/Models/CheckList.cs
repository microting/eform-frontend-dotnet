// The MIT License(MIT)
//
// Copyright(c) 2007-2017 microting
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CheckList : DbContext
    {
        public CheckList(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<check_lists> check_lists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<check_lists>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.case_type)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.folder_name)
                .IsUnicode(false);
        }
    }
}
