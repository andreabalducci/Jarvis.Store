using System;
using System.Collections.Generic;
using System.Linq;

namespace Jarvis.Store.Service.Shared
{
    public class ThemeState
    {
        public string CurrentTheme { get; set; } = "default";
    }


    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<MenuItem> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    public class MenuService
    {
        readonly MenuItem[] _items = new[]
        {
            new MenuItem()
            {
                Name = "Overview",
                Path = "/",
                Icon = "&#xe88a"
            },
            new MenuItem()
            {
                Name = "Dashboard",
                Path = "/dashboard",
                Icon = "&#xe871"
            },
        };

        public IEnumerable<MenuItem> Items => _items;

        public IEnumerable<MenuItem> Filter(string term)
        {
            if (string.IsNullOrEmpty(term))
                return _items;

            bool contains(string value) => value.Contains(term, StringComparison.OrdinalIgnoreCase);

            bool filter(MenuItem example) =>
                contains(example.Name) || (example.Tags != null && example.Tags.Any(contains));

            bool deepFilter(MenuItem example) => filter(example) || example.Children?.Any(filter) == true;

            return Items.Where(category => category.Children?.Any(deepFilter) == true)
                .Select(category => new MenuItem
                {
                    Name = category.Name,
                    Expanded = true,
                    Children = category.Children.Where(deepFilter).Select(example => new MenuItem
                        {
                            Name = example.Name,
                            Path = example.Path,
                            Icon = example.Icon,
                            Expanded = true,
                            Children = example.Children
                        }
                    ).ToArray()
                }).ToList();
        }

        public MenuItem FindCurrent(Uri uri)
        {
            return Items.SelectMany(example => example.Children ?? new[] { example })
                .FirstOrDefault(example => example.Path == uri.AbsolutePath || $"/{example.Path}" == uri.AbsolutePath);
        }

        public string TitleFor(MenuItem menuItem)
        {
            if (menuItem != null)
            {
                return menuItem.Title ?? menuItem.Name;
            }

            return "Jarvis Store";
        }

        public string DescriptionFor(MenuItem menuItem)
        {
            return menuItem?.Description ?? menuItem?.Name ?? "Empty";
        }
    }
}