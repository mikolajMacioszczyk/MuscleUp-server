package content.exampleController;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("test")
public class ExampleController {

    @GetMapping("/contentExample")
    String contentExample() {

        return "Content endpoint works fine";
    }
}
